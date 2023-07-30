using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class PlayerVisual:NetworkBehaviour
    {
        private void Start()
        {
            int index = GameManagerMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId);
            LayerMask playerLayerMask = GameManager.Instance.GetPlayerLayerMask(index);
            SetGameLayerRecursive(gameObject, playerLayerMask);
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
    }
}
