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
                GameManagerMultiplayer.Instance.StartHost();
                SceneLoader.LoadNetwork(SceneLoader.GameScene.TeamSelectScene);
            });

            startClientButton.onClick.AddListener(() => 
            {
                GameManagerMultiplayer.Instance.StartClient();
            });


        }

        private void Hide() => gameObject.SetActive(false);

    }
}
