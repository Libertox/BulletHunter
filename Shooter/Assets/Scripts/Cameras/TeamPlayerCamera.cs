using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class TeamPlayerCamera:NetworkBehaviour
    {
        private Camera teamPlayerCamera;

        private void Awake() => teamPlayerCamera = GetComponent<Camera>();
       
        private void Start()
        {
            if (!IsOwner) gameObject.SetActive(false);
            SetCullingMask();
        }

        private void SetCullingMask()
        {
            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
            LayerMask gunLayerMask = GameManager.Instance.GetPlayerTeamLayerMask(playerData.teamColorId);
            teamPlayerCamera.cullingMask |= (1 << gunLayerMask);
        }
    }
}
