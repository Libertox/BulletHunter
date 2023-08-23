using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter.UI
{
    public class PauseUI: MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private SettingsUI settingsUI;

        private bool isShow;

        private void Awake()
        {
            resumeButton.onClick.AddListener(() => 
            {
                Show();
                SoundManager.Instance.PlayButtonSound();
            });
            settingsButton.onClick.AddListener(() => 
            { 
                settingsUI.Show();
                SoundManager.Instance.PlayButtonSound();
            });
            exitButton.onClick.AddListener(() => 
            {
                PlayerController.ResetStaticData();
                PlayerStats.ResetStaticData();
                SoundManager.Instance.PlayButtonSound();
                SceneLoader.Load(SceneLoader.GameScene.MainMenu); 
            });
        }

        private void Start()
        {
            GameInput.Instance.OnPaused += GameInput_OnPaused;

            Hide();
        }

        private void GameInput_OnPaused(object sender, EventArgs e) => Show();
       
        private void Show()
        {
            if (!GameManager.Instance.CanPauseGame()) return;

            isShow = !isShow;

            if (GameManager.Instance.IsPauseState())
            {
                Cursor.lockState = CursorLockMode.Locked;
                GameManager.Instance.SetGameStateToPreviousGameState();
            }
            else
            {
                resumeButton.Select();
                Cursor.lockState = CursorLockMode.None;
                GameManager.Instance.SetGameState(GameManager.GameState.Pause);
            }
                
            gameObject.SetActive(isShow);
        }

        private void Hide() => gameObject.SetActive(false);

    }
}
