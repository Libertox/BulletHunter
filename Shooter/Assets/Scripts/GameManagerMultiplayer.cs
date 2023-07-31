using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

namespace Shooter
{
    public class GameManagerMultiplayer: NetworkBehaviour
    {
        public static GameManagerMultiplayer Instance { get; private set; }

        public event EventHandler OnPlayerDataNetworkListChanged;

        private NetworkList<PlayerData> playerDataNetworkList;

        [SerializeField] private List<Color> teamColorList;

        public int MaxPlayer { get; private set; }
        public NetworkVariable<int> MaxTeam { get; private set; } = new NetworkVariable<int>();
        public int PointsToWin { get; private set; }

        private void Awake()
        {
            Instance = this;

            playerDataNetworkList = new NetworkList<PlayerData>();
            playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;

            DontDestroyOnLoad(gameObject);


        }

  
        private void PlayerDataNetworkList_OnListChanged(NetworkListEvent<PlayerData> changeEvent)
        {
            OnPlayerDataNetworkListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void StartHost()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
            NetworkManager.Singleton.StartHost();
            
        }

        private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
        {
            for (int i = 0; i < playerDataNetworkList.Count; i++)
            {
                PlayerData playerData = playerDataNetworkList[i];
                if(playerData.clientId == clientId)
                {
                    playerDataNetworkList.RemoveAt(i);
                }
            }
        }

        private void NetworkManager_OnClientConnectedCallback(ulong clientId)
        {
            playerDataNetworkList.Add(new PlayerData
            {
                clientId = clientId,
                teamColorId = 0
            });

            SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
        }

        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();

            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
        }

        private void NetworkManager_Client_OnClientConnectedCallback(ulong obj)
        {
            SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

            PlayerData playerData = playerDataNetworkList[playerDataIndex];

            playerData.playerId = playerId;

            playerDataNetworkList[playerDataIndex] = playerData;
        }

        public bool IsPlayerIndexConnected(int playerIndex) => playerIndex < playerDataNetworkList.Count;

        public int GetPlayerDataIndexFromClientId(ulong playerId) 
        {
            for (int i = 0; i < playerDataNetworkList.Count; i++)
            {
                if (playerDataNetworkList[i].clientId == playerId)
                    return i;
            }
            return -1;
        }


        public PlayerData GetPlayerDataFromIndex(int playerIndex) => playerDataNetworkList[playerIndex];

        public Color GetTeamColor(int teamId) => teamColorList[teamId];

        public PlayerData GetPlayerDataFromClientId(ulong clientId)
        {
            foreach(PlayerData playerData in playerDataNetworkList)
            {
                if (playerData.clientId == clientId)
                    return playerData;
            }
            return default;
        }

   
        public PlayerData GetPlayerData() => GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);

        public void ChangePlayerTeamColor(int teamColorId)
        {
            ChangePlayerTeamColorServerRpc(teamColorId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangePlayerTeamColorServerRpc(int teamColorId, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

            PlayerData playerData = GetPlayerDataFromIndex(playerDataIndex);
            playerData.teamColorId = teamColorId;
            playerDataNetworkList[playerDataIndex] = playerData;
        }


        public void KickPlayer(ulong clientId)
        {
            NetworkManager.Singleton.DisconnectClient(clientId);
            NetworkManager_Server_OnClientDisconnectCallback(clientId);
        }


        public void SetGameSettings(int maxPlayer, int maxTeam, int pointsToWin)
        {
            MaxPlayer = maxPlayer;
            MaxTeam.Value = maxTeam;
            PointsToWin = pointsToWin;
        }
    }
}
