using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shooter.UI
{
    public class GameStartTimerUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameStartTimerText;

        private void Start()
        {
            GameManager.Instance.OnGameStartWaited += GameManager_OnGameStartWaited;
            GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;

            Hide();
        }

        private void GameManager_OnGameStarted(object sender, EventArgs e)
        {
            Hide();
        }

        private void GameManager_OnGameStartWaited(object sender, GameManager.OnGameStartWaitedEventArgs e)
        {
            Show();
            gameStartTimerText.SetText($"GAME WILL START {e.timerValue}");
        }


        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}
