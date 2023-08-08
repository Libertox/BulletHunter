using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter
{
    public class TeamSelectUI:MonoBehaviour
    {
        [SerializeField] private Button readyButton;
        [SerializeField] private Button mainMenuButton;

        [SerializeField] private TextMeshProUGUI lobbyNameText;
        [SerializeField] private TextMeshProUGUI lobbyCodeText;

        [SerializeField] private List<GameObject> selectTeamButton;
        
        private void Awake()
        {
            readyButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                TeamSelectManager.Instance.SetPlayerReady();      
            });

            mainMenuButton.onClick.AddListener(() =>
            {
                LobbyManager.Instance.LeaveLobby();
                SoundManager.Instance.PlayButtonSound();
                NetworkManager.Singleton.Shutdown();
                SceneLoader.Load(SceneLoader.GameScene.MainMenu);
            });
        }

        private void Start()
        {
            for (int i = 0; i < selectTeamButton.Count; i++)
            {
                if (i >= GameManagerMultiplayer.Instance.MaxTeam.Value)
                    selectTeamButton[i].SetActive(false);
            }

            Lobby lobby = LobbyManager.Instance.GetLobby();
            lobbyNameText.SetText(lobby.Name);
            lobbyCodeText.SetText("CODE: " + lobby.LobbyCode);
        }
    }
}
