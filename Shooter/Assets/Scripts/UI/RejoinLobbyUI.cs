using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter.UI
{
    public class RejoinLobbyUI:MonoBehaviour
    {
        [SerializeField] private Button rejoinButton;
        [SerializeField] private Button backMainMenuButton;

        private void Awake()
        {
            backMainMenuButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                Hide();
            });

            rejoinButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                LobbyManager.Instance.RejoinLobby();
                Hide();
            });

        }

       private async void Start()
       {
            Hide();

            if (await LobbyManager.Instance.HasActiveLobby())
                Show();
       }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

    }
}
