using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class GameManager: MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private GameState gameState = GameState.Play;

        public enum GameState
        {
            Pause,
            Play,
        }

        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }

        public void SetGameState(GameState gameState) => this.gameState = gameState;

        public bool IsPauseState() => gameState == GameState.Pause;

    }
}
