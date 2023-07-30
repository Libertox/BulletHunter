using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Shooter
{
    public class WeaponCamera: NetworkBehaviour
    {
        private Camera weaponCamera;

        private void Awake()
        {
            weaponCamera = GetComponent<Camera>();
        }

        private void Start()
        {
            if (!IsOwner)
            {
                gameObject.SetActive(false);
                return;
            }

            int index = GameManagerMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId);
            LayerMask gunLayerMask = GameManager.Instance.GetPlayerGunLayerMask(index);
            weaponCamera.cullingMask |= (1 << gunLayerMask);
        }

    }
}
