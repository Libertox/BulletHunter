using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    public class WeaponReloading : MonoBehaviour
    {
        public event EventHandler<OnReloadedEventArgs> OnReloaded;
        public event EventHandler OnCanelReloaded;

        public class OnReloadedEventArgs : EventArgs { public float reloadTime; }

        private bool isReload;
        private float time;

        private InventoryManager inventory;

        private void Awake() => inventory = GetComponent<InventoryManager>();
      
        private void Start()
        {
            GameInput.Instance.OnReloaded += GameInput_OnReloaded;
            GameInput.Instance.OnWeaponSelected += GameInput_OnWeaponSelected;
            GameInput.Instance.OnWeaponDroped += GameInput_OnWeaponDroped;
            GameInput.Instance.OnShooted += GameInput_OnShooted;

            if(PlayerStats.Instance != null)
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
            if (inventory.UseWeapon != null && inventory.UseWeapon.AmmoAmount <= 0)
                isReload = true;
        }

        private void GameInput_OnWeaponDroped(object sender, EventArgs e)
        {
            if (InventoryManager.Instance.UseWeapon != null)
                CancelReload();
        }

        private void GameInput_OnWeaponSelected(object sender, GameInput.OnWeaponSelectedEventArgs e) => CancelReload();
       
        private void GameInput_OnReloaded(object sender, EventArgs e)
        {
            if (inventory.UseWeapon != null)
                isReload = true;
        }

        private void Update()
        {
            if (!CanReload()) return;

            time += Time.deltaTime;
            OnReloaded?.Invoke(this, new OnReloadedEventArgs
            {
                reloadTime = time / inventory.UseWeapon.WeaponSO.ReloadTime
            });
            if (time > inventory.UseWeapon.WeaponSO.ReloadTime)
            {
                inventory.Reload();
                CancelReload();
            }
        }

        private bool CanReload() => isReload && inventory.UseWeapon != null && inventory.GetUseMagazine() > 0;

        private void CancelReload()
        {
            isReload = false;
            time = 0;
            OnCanelReloaded?.Invoke(this, EventArgs.Empty);
        }  
    }
}
