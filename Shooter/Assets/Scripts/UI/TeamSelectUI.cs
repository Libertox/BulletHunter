using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter
{
    public class TeamSelectUI:MonoBehaviour
    {
        [SerializeField] private Button playButton;

        private void Awake()
        {
            playButton.onClick.AddListener(() => SceneLoader.LoadNetwork(SceneLoader.GameScene.Game));
        }

    }
}
