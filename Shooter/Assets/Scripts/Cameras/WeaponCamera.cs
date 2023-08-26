using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace BulletHaunter.Cameras
{
    public class WeaponCamera: NetworkBehaviour
    {
        private Camera weaponCamera;

        private void Awake() => weaponCamera = GetComponent<Camera>();
        
        private void Start()
        {
            if (!IsOwner)
            {
                gameObject.SetActive(false);
                return;
            }
            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameManagerMultiplayer_OnPlayerDataNetworkListChanged;

            SetCullingMask();
        }

        private void GameManagerMultiplayer_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
        {
            SetCullingMask();
        }

        public override void OnDestroy()
        {
            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameManagerMultiplayer_OnPlayerDataNetworkListChanged;
        }
        private void SetCullingMask()
        {
            int index = GameManagerMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId);
            if (index == -1) return;
            LayerMask gunLayerMask = GameManager.Instance.GetPlayerGunLayerMask(index);
            weaponCamera.cullingMask = 0;
            weaponCamera.cullingMask |= (1 << gunLayerMask);
        }

     
    }
}
