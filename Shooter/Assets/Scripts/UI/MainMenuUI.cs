﻿using System;
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
        [SerializeField] private LobbyUI lobbyUI;

        [SerializeField] private Button characterSelectButton;

        private void Start()
        {
            playButton.Select();

            playButton.onClick.AddListener(() => 
            {
                lobbyUI.Show();
                SoundManager.Instance.PlayButtonSound();
            });
            settingsButton.onClick.AddListener(() => 
            {
                settingsUI.Show();
                SoundManager.Instance.PlayButtonSound();
            });
            exitButton.onClick.AddListener(() =>
            {
#if !UNITY_WEBGL
                Application.Quit();
                SoundManager.Instance.PlayButtonSound();
#endif
            });

            characterSelectButton.onClick.AddListener(() => 
            {
                SceneLoader.Load(SceneLoader.GameScene.CharacterSelectScene);
                SoundManager.Instance.PlayButtonSound();
            });
        }

    }
}
