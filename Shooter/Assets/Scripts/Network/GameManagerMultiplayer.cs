using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

namespace BulletHaunter
{
    public class GameManagerMultiplayer: NetworkBehaviour
    {
        public static GameManagerMultiplayer Instance { get; private set; }

        public const string PLAYER_PREFS_PLAYER_NAME = "PlayerName";
        public const string PLAYER_PREFS_CHOOSE_SKIN_INDEX = "ChooseSkinIndex";
        public const string PLAYER_PREFS_CHOOSE_TEAM = "ChooseTeam";
        public const string Default_Player_Name = "Player";

        public event EventHandler OnPlayerDataNetworkListChanged;

        private NetworkList<PlayerData> playerDataNetworkList;

        [SerializeField] private List<Color> teamColorList;
        [SerializeField] private List<string> teamNameList;
        [SerializeField] private PlayerSkinsSO playerSkinsSO;

        public NetworkVariable<int> MaxTeam { get; private set; } = new NetworkVariable<int>();
        public int PointsToWin { get; private set; }
        public int WinningTeam { get; private set; }

        private string playerName;
        private int playerSkin;

        private void Awake()
        {
            Instance = this;

            playerDataNetworkList = new NetworkList<PlayerData>();
            playerDataNetworkList.OnListChanged += PlayerDataNetworkList_OnListChanged;

            playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME, Default_Player_Name);
            playerSkin = PlayerPrefs.GetInt(PLAYER_PREFS_CHOOSE_SKIN_INDEX);

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
            NetworkManager.Singleton.OnServerStopped += NetworkManager_OnServerStopped;    

            NetworkManager.Singleton.StartHost();
    
        }

        private void NetworkManager_OnServerStopped(bool obj)
        {
            LobbyManager.Instance.DeleteLobby();
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
            SetPlayerNameServerRpc(playerName);
            SetPlayerSkinIdServerRpc(playerSkin);
            SetPlayerTeamIdServerRpc(PlayerPrefs.GetInt(PLAYER_PREFS_CHOOSE_TEAM, 0));
        }

     

        public void StartClient()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnectedCallback;
            NetworkManager.Singleton.StartClient();
        }

        private void NetworkManager_Client_OnClientConnectedCallback(ulong obj)
        {
            SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
            SetPlayerNameServerRpc(playerName);
            SetPlayerSkinIdServerRpc(playerSkin);
            SetPlayerTeamIdServerRpc(PlayerPrefs.GetInt(PLAYER_PREFS_CHOOSE_TEAM, 0));
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerTeamIdServerRpc(int teamId, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

            PlayerData playerData = playerDataNetworkList[playerDataIndex];

            playerData.teamColorId = teamId;

            playerDataNetworkList[playerDataIndex] = playerData;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

            PlayerData playerData = playerDataNetworkList[playerDataIndex];

            playerData.playerId = playerId;

            playerDataNetworkList[playerDataIndex] = playerData;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

            PlayerData playerData = playerDataNetworkList[playerDataIndex];

            playerData.playerName = playerName;

            playerDataNetworkList[playerDataIndex] = playerData;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerSkinIdServerRpc(int playerSkinId, ServerRpcParams serverRpcParams = default)
        {
            int playerDataIndex = GetPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);

            PlayerData playerData = playerDataNetworkList[playerDataIndex];

            playerData.playerSkinId = playerSkinId;

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

        public string GetTeamName(int teamId) => teamNameList[teamId];

        public Material GetPlayerMaterial(int skinIndex) => playerSkinsSO.PlayerSkinList[skinIndex];

        public PlayerData GetPlayerData() => GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);

        public PlayerData GetPlayerDataFromClientId(ulong clientId)
        {
            foreach(PlayerData playerData in playerDataNetworkList)
            {
                if (playerData.clientId == clientId)
                    return playerData;
            }
            return default;
        }

        public void ChangePlayerTeamColor(int teamColorId) 
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_CHOOSE_TEAM, teamColorId);
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

        public void SetPointsToWin(int pointsToWin) => PointsToWin = pointsToWin;

        public void SetMaxTeam(int maxTeam) => MaxTeam.Value = maxTeam;

        public void SetWinningTeam(int winningTeam) => WinningTeam = winningTeam;

        public void ResetPlayerTeam() => PlayerPrefs.DeleteKey(PLAYER_PREFS_CHOOSE_TEAM);

    }
}
