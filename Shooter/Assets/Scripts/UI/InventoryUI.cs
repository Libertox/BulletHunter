﻿using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class InventoryUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoAmountText;
        [SerializeField] private Image weaponIcon;
        [SerializeField] private BarUI weaponReloadBar;

        private void Start()
        {
            Inventory.Instance.OnAmmoChanged += Inventory_OnAmmoChanged;
            Inventory.Instance.OnSelectedWeaponChanged += Inventory_OnSelectedWeaponChanged;
            Inventory.Instance.OnReloaded += Inventory_OnReloaded;
            Inventory.Instance.OnCanelReloaded += Inventory_OnCanelReloaded;
            Inventory.Instance.OnSelectedWeaponDroped += Inventory_OnSelectedWeaponDroped;
            Hide();
        }

        private void Inventory_OnSelectedWeaponDroped(object sender, Inventory.OnSelectedWeaponChangedEventArgs e) => Hide();
       
        private void Inventory_OnCanelReloaded(object sender, EventArgs e)
        {
            weaponReloadBar.Hide();
        }

        private void Inventory_OnReloaded(object sender, Inventory.OnReloadedEventArgs e)
        {
            weaponReloadBar.ChangeFillAmount(e.reloadTime);
            weaponReloadBar.Show();
        }

        private void Inventory_OnSelectedWeaponChanged(object sender, EventArgs e)
        {
            Inventory inventory = sender as Inventory;
            if (!inventory.UseWeapon)
            {
                Hide();
                return;
            }
            Show();
            weaponIcon.sprite = inventory.UseWeapon.WeaponIcon;
            ChangeAmmoValueText(inventory);
        }

        private void Inventory_OnAmmoChanged(object sender, EventArgs e)
        {
            Inventory inventory = sender as Inventory;

            if (!inventory.UseWeapon) return;

            ChangeAmmoValueText(inventory);  
        }

        private void ChangeAmmoValueText(Inventory inventory) => ammoAmountText.SetText($"{inventory.GetUseAmmo()} / {inventory.GetUseMagazine()}");

        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);
    }
}
