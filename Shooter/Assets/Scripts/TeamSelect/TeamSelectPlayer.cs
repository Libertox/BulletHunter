using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Shooter
{
    public class TeamSelectPlayer:MonoBehaviour
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private GameObject readyText;
        [SerializeField] private Button kickButton;

        [SerializeField] private TeamSelectPlatform teamSelectPlatform;

        private void Awake()
        {
            kickButton.onClick.AddListener(() =>
            {
                PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromIndex(playerIndex);
                LobbyManager.Instance.KickLobby(playerData.playerId.ToString());
                GameManagerMultiplayer.Instance.KickPlayer(playerData.clientId);          
            });
        }

        private void Start()
        {
            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameManagerMultiplayer_OnPlayerDataNetworkListChanged;
            TeamSelectManager.Instance.OnReadyChanged += TeamSelectManager_OnReadyChanged;

            kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer && playerIndex != 0 );

            UpdatePlayer();
        }

        private void TeamSelectManager_OnReadyChanged(object sender, EventArgs e)
        {
            UpdatePlayer();
        }

        private void GameManagerMultiplayer_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
        {
            UpdatePlayer();
        }

        private void UpdatePlayer()
        {
            if (GameManagerMultiplayer.Instance.IsPlayerIndexConnected(playerIndex))
            {
                Show();

                PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromIndex(playerIndex);

                readyText.SetActive(TeamSelectManager.Instance.IsPlayerReady(playerData.clientId));

                teamSelectPlatform.SetColor(GameManagerMultiplayer.Instance.GetTeamColor(playerData.teamColorId));
            }
            else
            {
                Hide();
            }
        }

        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);
    }
}
