using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter.UI
{
    public class LobbyUI:MonoBehaviour
    {
        [SerializeField] private Button createGameButton;
        [SerializeField] private Button joinGameButton;

        [SerializeField] private CreateLobbyPanelUI createLobbyPanelUI;
        [SerializeField] private JoinLobbyPanelUI joinLobbyPanelUI;

        private void Awake()
        {
            createGameButton.onClick.AddListener(() => 
            {
                createLobbyPanelUI.Show();
                joinLobbyPanelUI.Hide();
                SoundManager.Instance.PlayButtonSound();
            });

            joinGameButton.onClick.AddListener(() => 
            {
                joinLobbyPanelUI.Show();
                createLobbyPanelUI.Hide();
                SoundManager.Instance.PlayButtonSound();
            });
        }

        private void Start()
        {
            GameInput.Instance.OnPaused += GameInput_OnPaused;

            Hide();
        }

        private void GameInput_OnPaused(object sender, EventArgs e)
        {
            LobbyManager.Instance.LeaveLobby();
            Hide();
        }

        public void Show() 
        {
            gameObject.SetActive(true);
            createGameButton.Select();
        } 

        private void Hide() => gameObject.SetActive(false);
    }
}
