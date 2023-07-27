using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter
{
    public class TestingNetcodeUI:MonoBehaviour
    {
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;

        private void Awake()
        {
            startHostButton.onClick.AddListener(() => 
            {
                NetworkManager.Singleton.StartHost();
                SceneLoader.LoadNetwork(SceneLoader.GameScene.TeamSelectScene);
            });

            startClientButton.onClick.AddListener(() => 
            {
                NetworkManager.Singleton.StartClient();
            });


        }

        private void Hide() => gameObject.SetActive(false);

    }
}
