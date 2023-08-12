using System;
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
                bool isPrivate = gameAccessDropdown.value != 0;
                GameManagerMultiplayer.Instance.SetGameSettings(maxPlayerDropdown.value + 1, maxTeamDropdown.value + 1, int.Parse(pointsToWinInputField.text));
                LobbyManager.Instance.CreateLobby(lobbyNameInputField.text, isPrivate, maxPlayerDropdown.value + 1);
                SoundManager.Instance.PlayButtonSound();
            });
        }


        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}
