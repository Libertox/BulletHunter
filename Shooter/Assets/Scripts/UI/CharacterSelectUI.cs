﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BulletHaunter.CharacterSelect;

namespace BulletHaunter.UI
{
    public class CharacterSelectUI:MonoBehaviour
    {
        [SerializeField] private Button backMainMenuButton;
        [SerializeField] private Button nextSkinButton;
        [SerializeField] private Button previousSkinButton;
        [SerializeField] private Button[] pointsButton;

        [SerializeField] private TMP_InputField playerNameInputField;

        private void Awake()
        {
            backMainMenuButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();
                CharacterSelectManager.Instance.SaveCharacterSelect(playerNameInputField.text);
                SceneLoader.Load(SceneLoader.GameScene.MainMenu);      
            });

            nextSkinButton.onClick.AddListener(() =>
            {
                CharacterSelectManager.Instance.SetNextSkin();
                UpdatePointsButton();
                SoundManager.Instance.PlayButtonSound();
            });

            previousSkinButton.onClick.AddListener(() =>
            {
                CharacterSelectManager.Instance.SetPreviousSkin();
                SoundManager.Instance.PlayButtonSound();
                UpdatePointsButton();
            });

            for (int i = 0; i < pointsButton.Length; i++)
            {
                int index = i;
                pointsButton[i].onClick.AddListener(() =>
                {
                    ResetPointsButtonColor();

                    int chooseSkinIndex = (CharacterSelectManager.Instance.ChooseSkinIndex / pointsButton.Length) * pointsButton.Length + index;
                    pointsButton[index].image.color = Color.gray;
                    CharacterSelectManager.Instance.SetSpecificSkin(chooseSkinIndex);
                    SoundManager.Instance.PlayButtonSound();
                });
            }
        }

        private void Start()
        {
            playerNameInputField.text = PlayerPrefs.GetString(GameManagerMultiplayer.PLAYER_PREFS_PLAYER_NAME, GameManagerMultiplayer.Default_Player_Name); ;
            UpdatePointsButton();
        }

        private void UpdatePointsButton()
        {
            ResetPointsButtonColor();

            pointsButton[CharacterSelectManager.Instance.ChooseSkinIndex % pointsButton.Length].image.color = Color.gray;
        }

        private void ResetPointsButtonColor()
        {
            foreach (var button in pointsButton)
                button.image.color = Color.white;
        }
    }
}
