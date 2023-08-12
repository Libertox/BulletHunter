using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter
{
    public class MessagePanelUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button continueButton;

        private void Awake()
        {
            continueButton.onClick.AddListener(() => 
            {
                Hide();
                SoundManager.Instance.PlayButtonSound();
            });
        }

        private void Start()
        {
            LobbyManager.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
            LobbyManager.Instance.OnJoinedLobbyFailed += LobbyManager_OnJoinedLobbyFailed;
            LobbyManager.Instance.OnQuickJoinedLobbyFailed += LobbyManager_OnQuickJoinedLobbyFailed;
            LobbyManager.Instance.OnCreatedLobby += LobbyManager_OnCreatedLobby;
            LobbyManager.Instance.OnCreatedLobbyFailed += LobbyManager_OnCreatedLobbyFailed;

            Hide();
        }

        private void LobbyManager_OnCreatedLobbyFailed(object sender, EventArgs e)
        {
            SetMessageText("FAILED TO CREATE LOBBY");
        }

        private void LobbyManager_OnCreatedLobby(object sender, EventArgs e)
        {
            SetMessageText("CREATING LOBBY...");
        }

        private void LobbyManager_OnQuickJoinedLobbyFailed(object sender, EventArgs e)
        {
            SetMessageText("COULD NOT FIND A LOBBY TO JOIN");
        }

        private void LobbyManager_OnJoinedLobbyFailed(object sender, EventArgs e)
        {
            SetMessageText("FAILED TO JOIN LOBBY!");
        }

        private void LobbyManager_OnJoinedLobby(object sender, EventArgs e)
        {
            SetMessageText("JOINING LOBBY... ");
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        private void SetMessageText(string message) 
        {
            Show();
            messageText.SetText(message);
        }

        private void OnDestroy()
        {
            LobbyManager.Instance.OnJoinedLobby -= LobbyManager_OnJoinedLobby;
            LobbyManager.Instance.OnJoinedLobbyFailed -= LobbyManager_OnJoinedLobbyFailed;
            LobbyManager.Instance.OnQuickJoinedLobbyFailed -= LobbyManager_OnQuickJoinedLobbyFailed;
            LobbyManager.Instance.OnCreatedLobby -= LobbyManager_OnCreatedLobby;
            LobbyManager.Instance.OnCreatedLobbyFailed -= LobbyManager_OnCreatedLobbyFailed;
        }

    }
}
