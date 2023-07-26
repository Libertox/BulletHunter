using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class GameManager: MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private List<Transform> playerSpawnPointsList;
       
        private List<int> usedPointsIndexList;
        private GameState gameState = GameState.Play;
        private GameState previousGameState;

        public bool  showPlayerName;

        [SerializeField] private List<WeaponSO> weaponSOList;

        public enum GameState
        {
            Pause,
            Play,
            Respawn
        }

        private void Awake()
        {
            if (!Instance)
                Instance = this;

            usedPointsIndexList = new List<int>();
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

    }
}
