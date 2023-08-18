
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class GameManager: NetworkBehaviour
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

        public class OnGameFinishedEventArgs:EventArgs { public Action gameFinishAction; }

        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private List<Transform> playerSpawnPointsList;
       
        private List<int> usedPointsIndexList;
        [SerializeField] private GameState gameState = GameState.Start;
        private GameState previousGameState;

        public bool ShowPlayerName { get; private set; }

        [SerializeField] private List<WeaponSO> weaponSOList;

        [SerializeField] private int[] playerLayerMask;
        [SerializeField] private int[] playerGunLayerMask;
        [SerializeField] private int[] playerTeamLayerMask;

        public SortedDictionary<int, int> TeamPointsDictionary;


        public enum GameState
        {
            Pause,
            Play,
            Respawn,
            Start,
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                foreach (var item in usedPointsIndexList)
                    Debug.Log(item);
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;

            if(PlayerStats.Instance != null)
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
            else
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;

            SetShowPlayerNick(PlayerPrefs.GetInt(PLAYER_PREFS_SHOW_PLAYER_NICK, 0) == 1);
            StartCoroutine(StartGame());
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
                PlayerStats.Instance.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
            }
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
                PlayerController playerController = Instantiate(playerPrefab, GetRandomPosition(), Quaternion.identity);
                playerController.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);      
            }
            ResetUsedPointsIndexList();
        }

        private IEnumerator StartGame()
        {
            float startTimer = gameStartCoolDown;
            OnGameStartWaited?.Invoke(this, new OnGameStartWaitedEventArgs { timerValue = startTimer });
            while (startTimer > 0)
            {
                yield return new WaitForSeconds(1);
                startTimer--;
                OnGameStartWaited?.Invoke(this, new OnGameStartWaitedEventArgs { timerValue = startTimer });
            }
            OnGameStarted?.Invoke(this, EventArgs.Empty);
            gameState = GameState.Play; 
        }

        public void SetGameState(GameState gameState) 
        {
            previousGameState = this.gameState;
            this.gameState = gameState;
        }

        public void SetGameStateToPreviousGameState() => gameState = previousGameState;

        public bool IsPauseState() => gameState == GameState.Pause;

        public bool IsStartState() => gameState == GameState.Start;

        public bool CanInputAction() => gameState == GameState.Play || gameState == GameState.Start;

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

        public LayerMask GetPlayerGunLayerMask(int index) => playerGunLayerMask[index];

        public LayerMask GetPlayerTeamLayerMask(int index) => playerTeamLayerMask[index];

        public void SetPointForTeam(ulong targetId, ulong ownerId) => SetPointsForTeamServerRpc(targetId, ownerId);

        [ServerRpc(RequireOwnership = false)]
        private void SetPointsForTeamServerRpc(ulong targetId, ulong ownerId)
        {
            PlayerData targetplayerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(targetId);
            PlayerData ownerPlayerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(ownerId);

            if(targetplayerData.teamColorId != ownerPlayerData.teamColorId)
                SetPointsForTeamClientRpc(targetplayerData.teamColorId);
        }

        [ClientRpc()]
        private void SetPointsForTeamClientRpc(int teamId)
        {
            TeamPointsDictionary[teamId]++;
            OnTeamPointsChanged?.Invoke(this, EventArgs.Empty);

            if (CheckAnyoneWin(teamId))
            {
                GameManagerMultiplayer.Instance.SetWinningTeam(teamId);
                OnGameFinished?.Invoke(this, new OnGameFinishedEventArgs
                {
                    gameFinishAction = () =>
                    {
                        SceneLoader.LoadNetwork(SceneLoader.GameScene.WinningScene);
                    }
                });
            }  
        }



        private bool CheckAnyoneWin(int teamId) => TeamPointsDictionary[teamId] >= GameManagerMultiplayer.Instance.PointsToWin;
     
        public void SetShowPlayerNick(bool isShowPlayerNick) 
        {
            ShowPlayerName = isShowPlayerNick;
            OnShowPlayerNickChanged?.Invoke(this, new OnShowPlayerNickChangedEventArgs { isShow = isShowPlayerNick });
        } 
    }
}
