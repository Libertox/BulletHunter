using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;

namespace BulletHaunter
{
    public class LobbyManager:MonoBehaviour
    {
        public static LobbyManager Instance { get; private set; }

        private Lobby joinedLobby;

        private float heartbeatTimer;
        private float listLobbiesTimer;

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
                float listLobbiesTimerMax = 3f;
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
                    float heartbearTimerMax = 15f;
                    heartbeatTimer = heartbearTimerMax;

                    LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
                }
            }
        }

        private bool IsLobbyHost()
        {
            return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        }

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

        public async void CreateLobby(string lobbyName, bool isPrivate, int playerNumber)
        {
            OnCreatedLobby?.Invoke(this, EventArgs.Empty);
            try
            {
                joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, playerNumber, new CreateLobbyOptions
                {
                    IsPrivate = isPrivate
                });

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
