using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class PlayerVisual:NetworkBehaviour
    {

        [SerializeField] private SkinnedMeshRenderer playerSkinnedMeshRenderer;

        private void Start()
        {
            int index = GameManagerMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId);
            LayerMask playerLayerMask = GameManager.Instance.GetPlayerLayerMask(index);
            SetGameLayerRecursive(gameObject, playerLayerMask);

            SetPlayerSkin();
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
    }
}
