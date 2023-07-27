using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class GameManager: NetworkBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private PlayerController playerPrefab;

        [SerializeField] private List<Transform> playerSpawnPointsList;
       
        private List<int> usedPointsIndexList;
        private GameState gameState = GameState.Play;
        private GameState previousGameState;

        public bool showPlayerName;

        [SerializeField] private List<WeaponSO> weaponSOList;

        [SerializeField] private int[] playerLayerMask;
        [SerializeField] private int[] playerGunLayerMask;

        private NetworkList<ulong> spawnedPlayerIdList;

        public enum GameState
        {
            Pause,
            Play,
            Respawn
        }

        private void Awake()
        {
           Instance = this;

            usedPointsIndexList = new List<int>();

            spawnedPlayerIdList = new NetworkList<ulong>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            }
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

        public void SetGameState(GameState gameState) 
        {
            previousGameState = this.gameState;
            this.gameState = gameState;
        }

        public void SetGameStateToPreviousGameState() => gameState = previousGameState;

        public bool IsPauseState() => gameState == GameState.Pause;

        public bool IsPlayState() => gameState == GameState.Play;

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
    }
}
