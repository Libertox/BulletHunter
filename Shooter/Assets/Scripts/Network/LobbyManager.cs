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

        private Lobby joinedLobby;

        private float heartbeatTimer;
        private readonly float heartbearTimerMax = 15f;

        private float listLobbiesTimer;
        private readonly float listLobbiesTimerMax = 3f;

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

            DontDestroyOnLoad(gameObject);

            InitializeUnityAuthentication();
        }

        private async void InitializeUnityAuthentication()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                InitializationOptions initializationOptions = new InitializationOptions();
                initializationOptions.SetProfile(UnityEngine.Random.Range(0, 1000).ToString());

                await UnityServices.InitializeAsync(initializationOptions);

                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

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
                       new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0",QueryFilter.OpOptions.GT)
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
                    IsPrivate = isPrivate
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

    }
}
