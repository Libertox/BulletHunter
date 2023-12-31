﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter.UI
{
    public class CreateLobbyPanelUI:MonoBehaviour
    {
        [SerializeField] private TMP_InputField lobbyNameInputField;
        [SerializeField] private TMP_InputField pointsToWinInputField;

        [SerializeField] private TMP_Dropdown gameAccessDropdown;
        [SerializeField] private TMP_Dropdown maxPlayerDropdown;
        [SerializeField] private TMP_Dropdown maxTeamDropdown;
 
        [SerializeField] private Button createLobby;

        private void Awake()
        {
            createLobby.onClick.AddListener(() => 
            {
                GameManagerMultiplayer.Instance.ResetPlayerTeam();

                bool isPrivate = gameAccessDropdown.value != 0;
                int pointsToWin = pointsToWinInputField.text == "" ? LobbyManager.DEFAULT_TEAM_POINTS : int.Parse(pointsToWinInputField.text);
                string lobbyName = lobbyNameInputField.text == "" ? LobbyManager.DEFAULT_LOBBY_NAME : lobbyNameInputField.text;

                GameManagerMultiplayer.Instance.SetPointsToWin(pointsToWin);
                GameManagerMultiplayer.Instance.SetMaxTeam(maxTeamDropdown.value + 1);
                LobbyManager.Instance.CreateLobby(lobbyName, isPrivate, maxPlayerDropdown.value + 1);
                SoundManager.Instance.PlayButtonSound();
            });
        }


        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}
