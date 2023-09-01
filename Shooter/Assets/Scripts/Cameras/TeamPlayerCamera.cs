using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter.Cameras
{
    public class TeamPlayerCamera:NetworkBehaviour
    {
        private Camera teamPlayerCamera;
        [SerializeField] private GameLayerMaskSO gameLayerMaskSO;

        private void Awake() => teamPlayerCamera = GetComponent<Camera>();
       
        private void Start()
        {
            if (!IsOwner) Hide();

            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameManagerMultiplayer_OnPlayerDataNetworkListChanged;

            SetCullingMask();        
        }

        private void Hide() => gameObject.SetActive(false);
       
        private void GameManagerMultiplayer_OnPlayerDataNetworkListChanged(object sender, EventArgs e) => SetCullingMask();
       
        public override void OnDestroy() =>
            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameManagerMultiplayer_OnPlayerDataNetworkListChanged;
       
        private void SetCullingMask()
        {
            PlayerData playerData = GameManagerMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
            LayerMask gunLayerMask = gameLayerMaskSO.PlayerTeamLayerMask[playerData.teamColorId];
            teamPlayerCamera.cullingMask = 0;
            teamPlayerCamera.cullingMask |= (1 << gunLayerMask);
        }

    }
}
