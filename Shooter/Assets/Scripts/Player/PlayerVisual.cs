using System;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class PlayerVisual:NetworkBehaviour
    {

        [SerializeField] private SkinnedMeshRenderer playerSkinnedMeshRenderer;
        [SerializeField] private TextMeshPro playerNickText;

        [SerializeField] private GameObject root;

        private void Start()
        {
            GameManager.Instance.OnShowPlayerNickChanged += GameManager_OnShowPlayerNickChanged;

            int index = GameManagerMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId);
            LayerMask playerLayerMask = GameManager.Instance.GetPlayerLayerMask(index);
            SetGameLayerRecursive(gameObject, playerLayerMask);
            root.layer = playerLayerMask;

            SetPlayerSkin();
            SetPlayerNick();
            SetPlayerNickShow(GameManager.Instance.ShowPlayerName);
        }

        private void GameManager_OnShowPlayerNickChanged(object sender, GameManager.OnShowPlayerNickChangedEventArgs e)
        {
            SetPlayerNickShow(e.isShow);
        }

        private void SetGameLayerRecursive(GameObject gameobject, int layer)
        {
            gameobject.layer = layer;
            foreach (Transform child in gameobject.transform)
            {
                child.gameObject.layer = layer;

                Transform _HasChildren = child.GetComponentInChildren<Transform>();
                if (_HasChildren != null)
                    SetGameLayerRecursive(child.gameObject, layer);

            }
        }

        private void SetPlayerSkin()
        {
            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
            Material material = GameManagerMultiplayer.Instance.GetPlayerMaterial(playerData.playerSkinId);
            playerSkinnedMeshRenderer.materials = new Material[]
            {
                material,
                material,
                material
            };
        }

        private void SetPlayerNick()
        {
            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
            playerNickText.SetText(playerData.playerName.ToString());
            playerNickText.color = GameManagerMultiplayer.Instance.GetTeamColor(playerData.teamColorId);
            playerNickText.gameObject.layer = GameManager.Instance.GetPlayerTeamLayerMask(playerData.teamColorId);
        }

        private void SetPlayerNickShow(bool isShow)
        {
            playerNickText.gameObject.SetActive(isShow);

            if (IsOwner) playerNickText.gameObject.SetActive(false);
        }
    }
}
