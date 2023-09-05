using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter.UI
{
    public class MainMenuUI: MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button characterSelectButton;

        [SerializeField] private SettingsUI settingsUI;
        [SerializeField] private LobbyUI lobbyUI;

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
                Application.Quit();
                SoundManager.Instance.PlayButtonSound();
            });

            characterSelectButton.onClick.AddListener(() => 
            {
                SceneLoader.Load(SceneLoader.GameScene.CharacterSelectScene);
                SoundManager.Instance.PlayButtonSound();
            });
        }

    }  
}
