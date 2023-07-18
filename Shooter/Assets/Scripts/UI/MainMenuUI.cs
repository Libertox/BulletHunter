using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class MainMenuUI: MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private SettingsUI settingsUI;


        private void Start()
        {
            Time.timeScale = 1f;
            playButton.Select();

            playButton.onClick.AddListener(() => SceneLoader.Load(SceneLoader.GameScene.LoadingScene));
            settingsButton.onClick.AddListener(() => { settingsUI.Show(); } );
            exitButton.onClick.AddListener(() =>
            {
#if !UNITY_WEBGL
                Application.Quit();
#endif
            });
        }

    }
}
