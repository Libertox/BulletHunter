using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHaunter
{
    public class TeamSelectPlayer:MonoBehaviour
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private GameObject readyText;
        [SerializeField] private Button kickButton;
        [SerializeField] private TextMeshPro playerNameText;
        [SerializeField] private SkinnedMeshRenderer playerSkinnedMeshRenderer;

        [SerializeField] private TeamSelectPlatform teamSelectPlatform;

        private void Awake()
        {
            kickButton.onClick.AddListener(() =>
            {
                PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromIndex(playerIndex);
                LobbyManager.Instance.KickLobby(playerData.playerId.ToString());
                GameManagerMultiplayer.Instance.KickPlayer(playerData.clientId);
                SoundManager.Instance.PlayButtonSound();
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

                playerNameText.SetText(playerData.playerName.ToString());

                Material material = GameManagerMultiplayer.Instance.GetPlayerMaterial(playerData.playerSkinId);
                playerSkinnedMeshRenderer.materials = new Material[]
                {
                    material,
                    material,
                    material,
                };

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
