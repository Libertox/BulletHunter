using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class TeamPlayerCamera:NetworkBehaviour
    {
        private Camera teamPlayerCamera;

        [SerializeField] private float unzoomValue;

        private void Awake() => teamPlayerCamera = GetComponent<Camera>();
       
        private void Start()
        {
            if (!IsOwner) gameObject.SetActive(false);
            SetCullingMask();

            GameInput.Instance.OnAimed += GameInput_OnAimed;
            GameInput.Instance.OnCancelAimed += GameInput_OnCancelAimed;
            GameInput.Instance.OnWeaponDroped += GameInput_OnCancelAimed;
        }

        private void GameInput_OnCancelAimed(object sender, EventArgs e) => Unzoom();

        private void GameInput_OnAimed(object sender, EventArgs e)
        {
            if (InventoryManager.Instance.UseWeapon != null)
                Zoom();
        }

        private void SetCullingMask()
        {
            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
            LayerMask gunLayerMask = GameManager.Instance.GetPlayerTeamLayerMask(playerData.teamColorId);
            teamPlayerCamera.cullingMask |= (1 << gunLayerMask);
        }

        private void Zoom() => teamPlayerCamera.fieldOfView = InventoryManager.Instance.UseWeapon.WeaponSO.WeaponZoom;

        private void Unzoom() => teamPlayerCamera.fieldOfView = unzoomValue;

    }
}
