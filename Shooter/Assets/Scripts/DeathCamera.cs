﻿using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class DeathCamera: NetworkBehaviour
    {

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
