using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter
{
    public class TeamSelectUI:MonoBehaviour
    {
        [SerializeField] private Button readyButton;
        [SerializeField] private Button mainMenuButton;

        private void Awake()
        {
            readyButton.onClick.AddListener(() => TeamSelectManager.Instance.SetPlayerReady());

            mainMenuButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.Shutdown();
                SceneLoader.Load(SceneLoader.GameScene.MainMenu);
            });
        }

    }
}
