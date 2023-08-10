using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class TeamPlayerCamera:NetworkBehaviour
    {
        [SerializeField] private Camera teamPlayerCamera;

        private void Start()
        {
            if (!IsOwner) gameObject.SetActive(false);

            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
            LayerMask gunLayerMask = GameManager.Instance.GetPlayerTeamLayerMask(playerData.teamColorId);
            teamPlayerCamera.cullingMask |= (1 << gunLayerMask);
        }


    }
}
