using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

namespace BulletHaunter
{
    public class LobbyManager:MonoBehaviour
    {
        public static LobbyManager Instance { get; private set; }

        private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";
        private const string LOBBY_DATA_AVAILABLE = "Available";
        private const string LOBBY_NOT_AVAILABLE = "Not available";
        private Lobby joinedLobby;

        private float heartbeatTimer;
        private readonly float heartbearTimerMax = 15f;

        private float listLobbiesTimer;
        private readonly float listLobbiesTimerMax = 3f;

        private List<string> joinedLobbiesIdList;

        public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;

        public event EventHandler OnJoinedLobby;
        public event EventHandler OnJoinedLobbyFailed;
        public event EventHandler OnQuickJoinedLobbyFailed;
        public event EventHandler OnCreatedLobby;
        public event EventHandler OnCreatedLobbyFailed;

        public class OnLobbyListChangedEventArgs : EventArgs { public List<Lobby> lobbyList; }

        private void Awake()
        {
            Instance = this;

            joinedLobby = null;

            DontDestroyOnLoad(gameObject);

            joinedLobbiesIdList = new List<string>();
        }

        private void Update()
        {
            HandleHeartbeat();
            HandlePeriodListLobbies();
        }

        private void HandlePeriodListLobbies()
        {
            if (joinedLobby != null || !AuthenticationService.Instance.IsSignedIn) return;


            listLobbiesTimer -= Time.deltaTime;
            if (listLobbiesTimer <= 0f)
            { 
                listLobbiesTimer = listLobbiesTimerMax;
                ListLobbies();
            }
        }

        private void HandleHeartbeat()
        {
            if (IsLobbyHost())
            {
                heartbeatTimer -= Time.deltaTime;
                if (heartbeatTimer <= 0f)
                {                  
                    heartbeatTimer = heartbearTimerMax;

                    LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
                }
            }
        }

        private bool IsLobbyHost() => joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
      
        private async void ListLobbies()
        {
            try
            {
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Filters = new List<QueryFilter>
                    {
                       new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT),
                       new QueryFilter(QueryFilter.FieldOptions.S1,LOBBY_DATA_AVAILABLE,QueryFilter.OpOptions.EQ)
                    }
                };

                QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

                OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
                {
                    lobbyList = queryResponse.Results
                });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async Task<Allocation> AllocateRelay(int playerNumber)
        {
            try
            {
                Allocation allocation =  await RelayService.Instance.CreateAllocationAsync(playerNumber);

                return allocation;
            }
            catch(RelayServiceException e)
            {
                Debug.Log(e);

                return default;
            }
           
        }

        private async Task<string> GetRelayJoinCode(Allocation allocation)
        {
            try
            {
                string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                return relayJoinCode;
            }
            catch(RelayServiceException e)
            {
                Debug.Log(e);
                return default;
            }
            
        }

        private async Task<JoinAllocation> JoinRelay(string joinCode)
        {
            try
            {
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                return joinAllocation;
            }
            catch(RelayServiceException e)
            {
                Debug.Log(e);
                return default;
            }
           
        }

        public async void CreateLobby(string lobbyName, bool isPrivate, int playerNumber)
        {
            OnCreatedLobby?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, playerNumber, new CreateLobbyOptions
                {
                    IsPrivate = isPrivate,
                    Data = new Dictionary<string, DataObject>
                    {
                        {LOBBY_DATA_AVAILABLE,new DataObject(DataObject.VisibilityOptions.Public,LOBBY_DATA_AVAILABLE,DataObject.IndexOptions.S1)}
                    }
                });
                Allocation allocation = await AllocateRelay(playerNumber);
                string relayJoinCode =  await GetRelayJoinCode(allocation);

                await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        {KEY_RELAY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode)}
                    }
                });

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

                GameManagerMultiplayer.Instance.StartHost();
                SceneLoader.LoadNetwork(SceneLoader.GameScene.TeamSelectScene);
            }
            catch (LobbyServiceException e)
            {
                OnCreatedLobbyFailed?.Invoke(this, EventArgs.Empty);
                Debug.Log(e);
            }

        }

        public async void QuickJoin()
        {
            OnJoinedLobby?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
                string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

                GameManagerMultiplayer.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                OnQuickJoinedLobbyFailed?.Invoke(this, EventArgs.Empty);
                Debug.Log(e);
            }

        }

        public async void JoinWithCode(string lobbyCode)
        {
            OnJoinedLobby?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

                if (joinedLobby.Data[LOBBY_DATA_AVAILABLE].Value == LOBBY_NOT_AVAILABLE)
                {
                    LeaveLobby();
                    OnJoinedLobbyFailed?.Invoke(this, EventArgs.Empty);
                    return;
                }

                string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

                GameManagerMultiplayer.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                OnJoinedLobbyFailed?.Invoke(this, EventArgs.Empty);
                Debug.Log(e);
            }
        }

        public async void JoinWithId(string lobbyId)
        {
            OnJoinedLobby?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);

                string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

                GameManagerMultiplayer.Instance.StartClient();
            }
            catch (LobbyServiceException e)
            {
                OnJoinedLobbyFailed?.Invoke(this, EventArgs.Empty);
                Debug.Log(e);
            }
        }

        public async void DeleteLobby()
        {
            if (joinedLobby != null)
            {
                try
                {
                    await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

                    joinedLobby = null;
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }

            }

        }

        public async void LeaveLobby()
        {
            if (joinedLobby != null)
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                    joinedLobby = null;
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }

        }

        public async void KickLobby(string playerId)
        {
            if (IsLobbyHost())
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }

        }

        public Lobby GetLobby() => joinedLobby;

        public void UpdateLobbyData()
        {
            try
            {
                LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
                {
                    IsPrivate = true,
                    Data = new Dictionary<string, DataObject>
                    {
                        {LOBBY_DATA_AVAILABLE,new DataObject(DataObject.VisibilityOptions.Public,LOBBY_NOT_AVAILABLE,DataObject.IndexOptions.S1)}
                    }
                });
            }
            catch(LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        public async Task<bool> HasActiveLobby()
        {
            try
            {
                joinedLobbiesIdList = await LobbyService.Instance.GetJoinedLobbiesAsync();
                return joinedLobbiesIdList.Count > 0;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                return false;
            }
            
        }

        public async void RejoinLobby()
        {
            OnJoinedLobby?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.ReconnectToLobbyAsync(joinedLobbiesIdList[0]);

                string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

                GameManagerMultiplayer.Instance.StartClient();

            }
            catch(LobbyServiceException e)
            {
                Debug.Log(e);
                OnJoinedLobbyFailed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
