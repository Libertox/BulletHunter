using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class DeathCamera: NetworkBehaviour
    {
        private Camera deathCamera;


        private void Awake()
        {
            deathCamera = GetComponent<Camera>();
        }

        private void Start()
        {
            if (!IsOwner) 
            {
                Hide();
                return;
            }

            if (PlayerStats.Instnace != null)
            {
                PlayerStats.Instnace.OnDeathed += PlayerStats_OnDeathed;
                PlayerStats.Instnace.OnRestored += PlayerStats_OnRestored;
            }
            else
            {
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
            }

            int index = GameManagerMultiplayer.Instance.GetIndexFromPlayerIdList(OwnerClientId);
            LayerMask gunLayerMask = GameManager.Instance.GetPlayerGunLayerMask(index);

            deathCamera.cullingMask &= ~(1 << gunLayerMask);
            
            Hide();
        }

      

        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instnace != null)
            {
                PlayerStats.Instnace.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instnace.OnDeathed += PlayerStats_OnDeathed;

                PlayerStats.Instnace.OnRestored -= PlayerStats_OnRestored;
                PlayerStats.Instnace.OnRestored += PlayerStats_OnRestored;
            }
        }

        private void PlayerStats_OnDeathed(object sender, EventArgs e) 
        {
            Show();
        }

        private void PlayerStats_OnRestored(object sender, EventArgs e)
        {
            Hide();
        }


        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);
    }
}
