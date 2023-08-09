using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter
{
    public class SettingsUI:MonoBehaviour
    {
        [SerializeField] private Button gameplaySettingsButton;
        [SerializeField] private Button keyBindingsSettingsButton;

        [SerializeField] private GameObject gameplaySettingsWindow;
        [SerializeField] private GameObject keyBindingsSettingsWindow;

        [SerializeField] private Slider musicSlider;
        [SerializeField] private TextMeshProUGUI musicValueText;

        [SerializeField] private Slider soundEffectsSlider;
        [SerializeField] private TextMeshProUGUI soundEffectsValueText;

        [SerializeField] private Slider mouseSensitivitySlider;
        [SerializeField] private TextMeshProUGUI mouseSentitivityValueText;

        [SerializeField] private Toggle showPlayerNameToggle;


        private void Awake()
        {
            gameplaySettingsButton.onClick.AddListener(() =>
            {
                gameplaySettingsWindow.SetActive(true);
                keyBindingsSettingsWindow.SetActive(false);
                SoundManager.Instance.PlayButtonSound();
            });

            keyBindingsSettingsButton.onClick.AddListener(() =>
            {
                gameplaySettingsWindow.SetActive(false);
                keyBindingsSettingsWindow.SetActive(true);
                SoundManager.Instance.PlayButtonSound();
            });

            musicSlider.onValueChanged.AddListener((float value) =>
            {
                musicValueText.SetText(value.ToString());
                MusicManager.Instance.SetMusicVolume(value * 0.1f);
            });

            soundEffectsSlider.onValueChanged.AddListener((float value) =>
            {
                soundEffectsValueText.SetText(value.ToString());
                SoundManager.Instance.SetSoundEffectVolume(value * 0.1f);
            });

            mouseSensitivitySlider.onValueChanged.AddListener((float value) =>
            {
                mouseSentitivityValueText.SetText(value.ToString());
                GameInput.Instance.SetMouseSensitivity(value * 10f);
            });

            showPlayerNameToggle.onValueChanged.AddListener((bool value) =>
            {
                //GameManager.Instance.showPlayerName = value;
            });

        }

        private void Start()
        {
            musicValueText.SetText(MusicManager.Instance.MusicVolume.ToString());
            musicSlider.value = MusicManager.Instance.MusicVolume;

            soundEffectsValueText.SetText($"{SoundManager.Instance.SoundEffectVolume * 10f}");
            soundEffectsSlider.value = SoundManager.Instance.SoundEffectVolume * 10f;

            mouseSensitivitySlider.value = GameInput.Instance.MouseSensitivity * 0.1f;
            mouseSentitivityValueText.SetText($"{GameInput.Instance.MouseSensitivity * 0.1f}");

            //showPlayerNameToggle.isOn = GameManager.Instance.showPlayerName;

            GameInput.Instance.OnPaused += GameInput_OnPaused;

            Hide();
        }

        private void GameInput_OnPaused(object sender, EventArgs e)
        {
            Hide();
        }

        public void Show() => gameObject.SetActive(true);

        public  void Hide() => gameObject.SetActive(false);

    }
}
