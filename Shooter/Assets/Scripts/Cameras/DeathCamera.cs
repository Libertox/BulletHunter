using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class DeathCamera: NetworkBehaviour
    {
        private Camera deathCamera;

        private void Awake() => deathCamera = GetComponent<Camera>();
     
        private void Start()
        {
            if (!IsOwner)
            {
                Hide();
                return;
            }

            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
                PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;
            }
            else
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
 
            SetCullingMask();

            Hide();
        }

        private void SetCullingMask()
        {
            int index = GameManagerMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId);
            LayerMask gunLayerMask = GameManager.Instance.GetPlayerGunLayerMask(index);

            deathCamera.cullingMask &= ~(1 << gunLayerMask);
        }


        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;

                PlayerStats.Instance.OnRestored -= PlayerStats_OnRestored;
                PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;
            }
        }

        private void PlayerStats_OnDeathed(object sender, EventArgs e) => Show();
      
        private void PlayerStats_OnRestored(object sender, EventArgs e) => Hide();
       
        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}
