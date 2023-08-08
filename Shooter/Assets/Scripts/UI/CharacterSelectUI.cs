using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class CharacterSelectUI:MonoBehaviour
    {
        [SerializeField] private Button backMainMenuButton;
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private Button nextSkinButton;
        [SerializeField] private Button previousSkinButton;

        [SerializeField] private Button[] pointsButton;


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
                    int newIndex = (CharacterSelectManager.Instance.ChooseSkinIndex / pointsButton.Length) * pointsButton.Length + index;
                    CharacterSelectManager.Instance.SetChooseSkinIndex(newIndex);
                    SoundManager.Instance.PlayButtonSound();
                });


            }
        }

        private void Start()
        {
            playerNameInputField.text = CharacterSelectManager.Instance.GetPlayerName();
            UpdatePointsButton();
        }


        private void UpdatePointsButton()
        {
            pointsButton[CharacterSelectManager.Instance.ChooseSkinIndex % pointsButton.Length].Select();
        }
    }
}
