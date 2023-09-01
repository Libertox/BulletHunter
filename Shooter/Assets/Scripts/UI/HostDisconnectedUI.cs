using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace BulletHaunter.UI
{
    public class HostDisconnectedUI:MonoBehaviour
    {

        [SerializeField] private Button backMainMenuButton;

        private void Awake()
        {
            backMainMenuButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                PlayerStats.ResetStaticData();
                PlayerController.ResetStaticData();
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
            if (NetworkManager.Singleton != null)
                NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }

        private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
        {
            if(clientId == NetworkManager.ServerClientId)
            {
                Show();
                Cursor.lockState = CursorLockMode.None;
                GameManager.Instance.SetGameState(GameManager.GameState.HostExit);
            }  
        }

        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);
    }
}
