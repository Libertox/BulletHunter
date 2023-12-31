﻿using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter.UI
{
    public class KickPlayerUI: MonoBehaviour
    {
        [SerializeField] private Button backMainMenuButton;

        private void Awake()
        {
            backMainMenuButton.onClick.AddListener(() =>
            {
                LobbyManager.Instance.LeaveLobby();
                SoundManager.Instance.PlayButtonSound();
                SceneLoader.Load(SceneLoader.GameScene.MainMenu);
            });
        }

        private void Start()
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;

            Hide();
        }

        private void OnDestroy()
        {
            if(NetworkManager.Singleton != null)
                NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId) 
        {
            if(!NetworkManager.Singleton.IsServer)
                Show();
        } 
      
        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);


    }
}
