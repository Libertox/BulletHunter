using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter.UI
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
                if (!TeamSelectManager.Instance.IsPlayerReady(NetworkManager.Singleton.LocalClientId))
                {
                    GameManagerMultiplayer.Instance.ChangePlayerTeamColor(teamColorId);
                    SoundManager.Instance.PlayButtonSound();
                }               
            });
        }

        private void Start()
        {
            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameManagerMultiplayer_OnPlayerDataNetworkListChanged;
            image.color = GameManagerMultiplayer.Instance.GetTeamColor(teamColorId);
            UpdateIsSelected();
        }

        private void OnDestroy() => 
            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameManagerMultiplayer_OnPlayerDataNetworkListChanged;
        

        private void GameManagerMultiplayer_OnPlayerDataNetworkListChanged(object sender, EventArgs e) => UpdateIsSelected();
       
        private void UpdateIsSelected()
        {
            if (GameManagerMultiplayer.Instance.GetPlayerData().teamColorId == teamColorId)
                selectedImage.SetActive(true);
            else
                selectedImage.SetActive(false);
        }

    }
}
