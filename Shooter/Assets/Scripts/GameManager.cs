
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class GameManager: NetworkBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event EventHandler<OnGameStartWaitedEventArgs> OnGameStartWaited;
        public event EventHandler OnGameStarted;

        public class OnGameStartWaitedEventArgs : EventArgs { public float timerValue; };

        [SerializeField] private PlayerController playerPrefab;
        [SerializeField] private List<Transform> playerSpawnPointsList;
       
        private List<int> usedPointsIndexList;
        private GameState gameState = GameState.Start;
        private GameState previousGameState;

        public bool showPlayerName;

        [SerializeField] private List<WeaponSO> weaponSOList;

        [SerializeField] private int[] playerLayerMask;
        [SerializeField] private int[] playerGunLayerMask;

        private NetworkList<ulong> spawnedPlayerIdList;
        public event EventHandler OnTeamPointsChanged;
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

            spawnedPlayerIdList = new NetworkList<ulong>();

            TeamPointsDictionary = new SortedDictionary<int, int>();

            for (int i = 0; i < GameManagerMultiplayer.Instance.MaxTeam.Value; i++)
            {
                TeamPointsDictionary[i] = i;
            }
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            }

            if(PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
            }
            else
            {
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
            }

            StartCoroutine(StartGame());
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
                if (!spawnedPlayerIdList.Contains(clientId))
                {
                    PlayerController playerController = Instantiate(playerPrefab);
                    playerController.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
                    spawnedPlayerIdList.Add(clientId);
                }
            }

        }

        private IEnumerator StartGame()
        {
            float startTimer = 30;
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
            while (usedPointsIndexList.Contains(randomIndex) && usedPointsIndexList.Capacity < playerSpawnPointsList.Capacity)
            {
                randomIndex = UnityEngine.Random.Range(0, playerSpawnPointsList.Capacity);
            }
            usedPointsIndexList.Add(randomIndex);
            return playerSpawnPointsList[randomIndex].position;

        }

        public int GetWeaponSOIndex(WeaponSO weaponSO)
        {
            return weaponSOList.IndexOf(weaponSO);
        }

        public WeaponSO GetWeaponSOFromIndex(int index)
        {
            return weaponSOList[index];
        }


        public LayerMask GetPlayerLayerMask(int index) => playerLayerMask[index];

        public LayerMask GetPlayerGunLayerMask(int index) => playerGunLayerMask[index];

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
        }
    }
}
