using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class PlayerReloading : NetworkBehaviour
    {
        public static event EventHandler<OnReloadedEventArgs> OnReloaded;
        public static event EventHandler OnCanelReloaded;

        public class OnReloadedEventArgs : EventArgs { public float reloadTime; }

        private bool isReload;
        private float time;

        private void Start()
        {
            if (!IsOwner) return;

            GameInput.Instance.OnReloaded += GameInput_OnReloaded;
            GameInput.Instance.OnWeaponSelected += GameInput_OnWeaponSelected;
            GameInput.Instance.OnWeaponDroped += GameInput_OnWeaponDroped;
            GameInput.Instance.OnShooted += GameInput_OnShooted;

            if (PlayerStats.Instance != null)
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
            else
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
        }

        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
            }
        }

        private void PlayerStats_OnDeathed(object sender, EventArgs e) => CancelReload();

        private void GameInput_OnShooted(object sender, EventArgs e)
        {
            if (InventoryManager.Instance.UseWeapon != null && InventoryManager.Instance.UseWeapon.AmmoAmount <= 0)
                isReload = true;
        }

        private void GameInput_OnWeaponDroped(object sender, EventArgs e)
        {
           CancelReload();
        }

        private void GameInput_OnWeaponSelected(object sender, GameInput.OnWeaponSelectedEventArgs e) => CancelReload();

        private void GameInput_OnReloaded(object sender, EventArgs e)
        {
            if (InventoryManager.Instance.UseWeapon != null)
                isReload = true;
        }

        private void Update()
        {
            if (!IsOwner || !CanReload()) return;

            time += Time.deltaTime;
            OnReloaded?.Invoke(this, new OnReloadedEventArgs
            {
                reloadTime = time / InventoryManager.Instance.UseWeapon.WeaponSO.ReloadTime
            });
            if (time > InventoryManager.Instance.UseWeapon.WeaponSO.ReloadTime)
            {
                InventoryManager.Instance.Reload();
                CancelReload();
            }
        }

        private bool CanReload() => isReload && InventoryManager.Instance.UseWeapon != null && InventoryManager.Instance.GetUseMagazine() > 0;

        private void CancelReload()
        {
            isReload = false;
            time = 0;
            OnCanelReloaded?.Invoke(this, EventArgs.Empty);
        }

    }
}
