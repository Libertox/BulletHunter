using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class JoinLobbyPanelUI:MonoBehaviour
    {
        [SerializeField] private Button quickJoinButton;

        [SerializeField] private Button joinByCodeButton;
        [SerializeField] private TMP_InputField joinCodeInputField;

        [SerializeField] private LobbySlotUI lobbySlotTemplate;
        [SerializeField] private Transform containerTitle;

        [SerializeField] private RectTransform container;

        private void Awake()
        {
            quickJoinButton.onClick.AddListener(() => LobbyManager.Instance.QuickJoin());

            joinByCodeButton.onClick.AddListener(() => LobbyManager.Instance.JoinWithCode(joinCodeInputField.text));
        }

        private void Start()
        {
            LobbyManager.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
            UpdateLobbyList(new List<Lobby>());
            Hide();
        }

        private void LobbyManager_OnLobbyListChanged(object sender, LobbyManager.OnLobbyListChangedEventArgs e)
        {
            UpdateLobbyList(e.lobbyList);
        }

        private void OnDestroy()
        {
            LobbyManager.Instance.OnLobbyListChanged -= LobbyManager_OnLobbyListChanged;
        }

        private void UpdateLobbyList(List<Lobby> availableLobbies)
        {
            lobbySlotTemplate.Hide();

            foreach(Transform child in container)
            {
                if (child == containerTitle.transform || child == lobbySlotTemplate.transform) continue;

                Destroy(child.gameObject);
            }
            container.sizeDelta = new Vector2(0, 100);


            foreach(Lobby lobby in availableLobbies)
            {
                LobbySlotUI lobbySlotUI = Instantiate(lobbySlotTemplate, container);
                lobbySlotUI.Show();
                lobbySlotUI.SetLobby(lobby);
                container.sizeDelta = new Vector2(0, container.rect.height + 150);
            }

        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);
    }
}
