using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;


namespace BulletHaunter
{
    public class GameManager : NetworkBehaviour
    {
        public const string PLAYER_PREFS_SHOW_PLAYER_NICK = "showPlayerNick";
        private const int gameStartCoolDown = 30;

        public static GameManager Instance { get; private set; }

        public event EventHandler<OnGameStartWaitedEventArgs> OnGameStartWaited;
        public event EventHandler OnGameStarted;
        public event EventHandler OnTeamPointsChanged;
        public event EventHandler<OnShowPlayerNickChangedEventArgs> OnShowPlayerNickChanged;
        public event EventHandler<OnGameFinishedEventArgs> OnGameFinished;

        public class OnGameStartWaitedEventArgs : EventArgs { public float timerValue; };

        public class OnShowPlayerNickChangedEventArgs : EventArgs { public bool isShow; }

        public class OnGameFinishedEventArgs : EventArgs { public Action gameFinishAction; }

        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private List<Transform> playerSpawnPointsList;

        private List<int> usedPointsIndexList;
        private GameState gameState = GameState.Play;
        private GameState previousGameState;
        private NetworkVariable<float> startGameTimer = new NetworkVariable<float>(0f);

        public bool ShowPlayerName { get; private set; }

        [SerializeField] private List<WeaponSO> weaponSOList;

        [SerializeField] private int[] playerLayerMask;
        [SerializeField] private int[] playerGunLayerMask;
        [SerializeField] private int[] playerTeamLayerMask;

        public SortedDictionary<int, int> TeamPointsDictionary { get; private set; }

        public event EventHandler OnPlayerReconnected;

        public enum GameState
        {
            Pause,
            Play,
            Respawn,
            WaitingGame,
            HostExit,
        }

        private void Awake()
        {
            Instance = this;

            usedPointsIndexList = new List<int>();

            TeamPointsDictionary = new SortedDictionary<int, int>();

            for (int i = 0; i < GameManagerMultiplayer.Instance.MaxTeam.Value; i++)
            {
                TeamPointsDictionary[i] = 0;
            }

        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
                NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
                StartCoroutine(StartGameCoroutine());
            }

            startGameTimer.OnValueChanged += StartGameTimer_OnValueChanged;

            if (PlayerStats.Instance != null)
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
            else
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;

            SetShowPlayerNick(PlayerPrefs.GetInt(PLAYER_PREFS_SHOW_PLAYER_NICK, 0) == 1);
        }

      
        private void StartGameTimer_OnValueChanged(float previousValue, float newValue)
        {
            if(newValue > 0)
            {
                OnGameStartWaited?.Invoke(this, new OnGameStartWaitedEventArgs { timerValue = newValue });

                if (gameState == GameState.Play)
                    gameState = GameState.WaitingGame;
            }   
            else
            {
                OnGameStarted?.Invoke(this, EventArgs.Empty);
                if (IsPauseState())
                {
                    gameState = GameState.Pause;
                    previousGameState = GameState.Play;
                }
                else
                {
                    gameState = GameState.Play;
                }               
            }
        }

        private void NetworkManager_OnClientConnectedCallback(ulong clientId)
        {
            PlayerController playerController = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            playerController.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

            PlayerReconnectedClientRpc();
            SendTeamPointsValueClientRpc(TeamPointsDictionary.Values.ToArray());      
        }

        [ClientRpc]
        private void PlayerReconnectedClientRpc()
        {
            OnPlayerReconnected?.Invoke(this, EventArgs.Empty);    
        }

        [ClientRpc]
        private void SendTeamPointsValueClientRpc(int[] teamPointsValue)
        {
            for (int i = 0; i < GameManagerMultiplayer.Instance.MaxTeam.Value; i++)
                TeamPointsDictionary[i] = teamPointsValue[i];
        }

        public override void OnDestroy()
        {
            if (IsServer && NetworkManager.Singleton != null)
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
        }
      
        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnRestored -= PlayerStats_OnRestored;
                PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;


                PlayerStats.Instance.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
            }
        }

        private void PlayerStats_OnRestored(object sender, EventArgs e)
        {
            PlayerStats playerController = sender as PlayerStats;
            playerController.transform.position = GetRandomPosition();
            SetGameState(GameState.Play);
        }

        private void PlayerStats_OnDeathed(object sender, PlayerStats.OnDeathedEventArgs e)
        {
            SetPointForTeam(e.targetId,e.ownerId);
            SetGameState(GameState.Respawn);
        }

        private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                PlayerController playerController = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                playerController.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
            }
            ResetUsedPointsIndexList();
        }

        private IEnumerator StartGameCoroutine()
        {
            startGameTimer.Value = gameStartCoolDown;
            while (startGameTimer.Value > 0)
            {
                yield return new WaitForSeconds(1);
                startGameTimer.Value--;
            }
        }

        public void SetGameState(GameState gameState) 
        {
            previousGameState = this.gameState;
            this.gameState = gameState;
        }

        public void SetGameStateToPreviousGameState() => gameState = previousGameState;

        public bool IsPauseState() => gameState == GameState.Pause;

        public bool IsStartState() => gameState == GameState.WaitingGame || previousGameState == GameState.WaitingGame;

        public bool CanInputAction() => gameState == GameState.Play || gameState == GameState.WaitingGame;

        public bool CanPauseGame() => gameState != GameState.HostExit;

        public Vector3 GetRandomPosition()
        {
            int randomIndex = UnityEngine.Random.Range(0, playerSpawnPointsList.Capacity);
            ResetUsedPointsIndexList();
            while (usedPointsIndexList.Contains(randomIndex))
            {
                randomIndex = UnityEngine.Random.Range(0, playerSpawnPointsList.Capacity);
            }
            AddIndexToUsedPointsListClientRpc(randomIndex);
            return playerSpawnPointsList[randomIndex].position;
        }

        [ClientRpc()]
        private void AddIndexToUsedPointsListClientRpc(int index) 
        {
            usedPointsIndexList.Add(index);
        } 

        private void ResetUsedPointsIndexList()
        {
            if (usedPointsIndexList.Count >= playerSpawnPointsList.Count)
                usedPointsIndexList.Clear();
        }

        public int GetWeaponSOIndex(WeaponSO weaponSO) => weaponSOList.IndexOf(weaponSO);
      
        public WeaponSO GetWeaponSOFromIndex(int index) => weaponSOList[index];
 
        public LayerMask GetPlayerLayerMask(int index) => playerLayerMask[index];

        public int GetPlayerLayerMaskLength() => playerLayerMask.Length;

        public LayerMask GetPlayerGunLayerMask(int index) => playerGunLayerMask[index];

        public int GetPlayerGunLayerMaskLength() => playerGunLayerMask.Length;

        public LayerMask GetPlayerTeamLayerMask(int index) => playerTeamLayerMask[index];

        public void SetPointForTeam(ulong targetId, ulong ownerId) => SetPointsForTeamServerRpc(targetId, ownerId);

        [ServerRpc(RequireOwnership = false)]
        private void SetPointsForTeamServerRpc(ulong targetId, ulong ownerId)
        {
            PlayerData targetplayerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(targetId);
            PlayerData ownerPlayerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(ownerId);

            if(targetplayerData.teamColorId != ownerPlayerData.teamColorId)
                SetPointsForTeamClientRpc(targetplayerData.teamColorId);

            if (CheckAnyoneWin(targetplayerData.teamColorId))
            {
                FinishGameClientRpc(targetplayerData.teamColorId);
                LobbyManager.Instance.DeleteLobby();
            }
                
        }

        [ClientRpc()]
        private void SetPointsForTeamClientRpc(int teamId)
        {
            TeamPointsDictionary[teamId]++;
            OnTeamPointsChanged?.Invoke(this, EventArgs.Empty); 
        }

        [ClientRpc]
        private void FinishGameClientRpc(int winningTeam)
        {
            GameManagerMultiplayer.Instance.SetWinningTeam(winningTeam);
            OnGameFinished?.Invoke(this, new OnGameFinishedEventArgs
            {
                gameFinishAction = () =>
                {
                    SceneLoader.LoadNetwork(SceneLoader.GameScene.WinningScene);
                }
            });
        }



        private bool CheckAnyoneWin(int teamId) => TeamPointsDictionary[teamId] >= GameManagerMultiplayer.Instance.PointsToWin;
     
        public void SetShowPlayerNick(bool isShowPlayerNick) 
        {
            ShowPlayerName = isShowPlayerNick;
            OnShowPlayerNickChanged?.Invoke(this, new OnShowPlayerNickChangedEventArgs { isShow = isShowPlayerNick });
        } 
    }
}
