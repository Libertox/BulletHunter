using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shooter
{
    public class WinScenePlayerVisual:MonoBehaviour
    {
        [SerializeField] private int playerIndex;
        [SerializeField] private TextMeshPro playerNameText;
        [SerializeField] private SkinnedMeshRenderer playerSkinnedMeshRenderer;

        private void Start()
        {
            UpdatePlayer();
        }

        private void UpdatePlayer()
        {
            if (!GameManagerMultiplayer.Instance.IsPlayerIndexConnected(playerIndex)) 
            {
                Hide();
                return;
            }

            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromIndex(playerIndex);
            if (playerData.teamColorId == GameManagerMultiplayer.Instance.WinningTeam)
            {
                Show();
                playerNameText.SetText(playerData.playerName.ToString());
                Material material = GameManagerMultiplayer.Instance.GetPlayerMaterial(playerData.playerSkinId);
                playerSkinnedMeshRenderer.materials = new Material[]
                {
                    material,
                    material,
                    material,
                };
            }
            else
                Hide();
               
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}
