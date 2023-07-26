using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
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
            resumeButton.onClick.AddListener(() => Show());
            settingsButton.onClick.AddListener(() => { settingsUI.Show(); });
            exitButton.onClick.AddListener(() => 
            {
                PlayerController.ResetStaticData();
                WeaponReloading.ResetStaticData();
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
            isShow = !isShow;

            if (Time.timeScale == 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                GameManager.Instance.SetGameStateToPreviousGameState();
                Time.timeScale = 1;
            }
            else
            {
                resumeButton.Select();
                Cursor.lockState = CursorLockMode.None;
                GameManager.Instance.SetGameState(GameManager.GameState.Pause);
                Time.timeScale = 0;
            }
                
            gameObject.SetActive(isShow);
        }

        private void Hide() => gameObject.SetActive(false);


    }
}
