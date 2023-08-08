using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class TeamColorSelectSingleUI:MonoBehaviour
    {
        [SerializeField] private int teamColorId;
        [SerializeField] private Image image;
        [SerializeField] private GameObject selectedImage;

        [SerializeField] private Button changeTeamColorButton;

        private void Awake()
        {
            changeTeamColorButton.onClick.AddListener(() => 
            {
                GameManagerMultiplayer.Instance.ChangePlayerTeamColor(teamColorId);
                SoundManager.Instance.PlayButtonSound();
            });
        }

        private void Start()
        {
            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameManagerMultiplayer_OnPlayerDataNetworkListChanged;
            image.color = GameManagerMultiplayer.Instance.GetTeamColor(teamColorId);
            UpdateIsSelected();
        }

        private void GameManagerMultiplayer_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
        {
            UpdateIsSelected();
        }

        private void UpdateIsSelected()
        {
            if (GameManagerMultiplayer.Instance.GetPlayerData().teamColorId == teamColorId)
                selectedImage.SetActive(true);
            else
                selectedImage.SetActive(false);
        }

    }
}
