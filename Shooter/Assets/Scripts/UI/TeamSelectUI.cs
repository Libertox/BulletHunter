﻿using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter
{
    public class TeamSelectUI:MonoBehaviour
    {
        [SerializeField] private Button readyButton;
        [SerializeField] private Button unreadyButton;
        [SerializeField] private Button mainMenuButton;

        [SerializeField] private TextMeshProUGUI lobbyNameText;
        [SerializeField] private TextMeshProUGUI lobbyCodeText;
        [SerializeField] private TextMeshProUGUI lobbyAdminText;

        [SerializeField] private List<GameObject> selectTeamButton;
        
        private void Awake()
        {
            readyButton.onClick.AddListener(() => 
            {
                SoundManager.Instance.PlayButtonSound();
                TeamSelectManager.Instance.SetPlayerReady();

                unreadyButton.gameObject.SetActive(true);
                readyButton.gameObject.SetActive(false);
            });

            unreadyButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                TeamSelectManager.Instance.SetPlayerUnready();

                unreadyButton.gameObject.SetActive(false);
                readyButton.gameObject.SetActive(true);
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
            UpdateTeamSelectButton();

            UpdateLobbyInformation();
        }

        private void UpdateLobbyInformation()
        {
            Lobby lobby = LobbyManager.Instance.GetLobby();
            lobbyNameText.SetText(lobby.Name);
            lobbyCodeText.SetText("CODE: " + lobby.LobbyCode);
            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(0);
            lobbyAdminText.SetText("ADMIN: " + playerData.playerName.ToString());
        }

        private void UpdateTeamSelectButton()
        {
            for (int i = 0; i < selectTeamButton.Count; i++)
            {
                if (i >= GameManagerMultiplayer.Instance.MaxTeam.Value)
                    selectTeamButton[i].SetActive(false);
            }
        }
    }
}
