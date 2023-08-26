using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter
{
    public class LobbySlotUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyName;
        [SerializeField] private TextMeshProUGUI playerNumber;
        [SerializeField] private Button joinLobbyButton;

        private Lobby lobby;

        private void Awake()
        {
            joinLobbyButton.onClick.AddListener(() => 
            {
                LobbyManager.Instance.JoinWithId(lobby.Id);
                SoundManager.Instance.PlayButtonSound();
                GameManagerMultiplayer.Instance.ResetPlayerTeam();
            });
        }

        public void SetLobby(Lobby lobby)
        {
            this.lobby = lobby;
            lobbyName.SetText(lobby.Name);
            playerNumber.SetText($"{lobby.MaxPlayers - lobby.AvailableSlots} / {lobby.MaxPlayers}");
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
        
    }
}
