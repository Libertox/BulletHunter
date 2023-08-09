using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace BulletHaunter.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoAmountText;
        [SerializeField] private Image weaponIcon;
        [SerializeField] private BarUI weaponReloadBar;

        [SerializeField] private GameObject[] grenadeAmountIcon;

        private void Start()
        {
            InventoryManager.Instance.OnAmmoChanged += Inventory_OnAmmoChanged;
            InventoryManager.Instance.OnSelectedWeaponChanged += Inventory_OnSelectedWeaponChanged;

            WeaponReloading.OnReloaded += WeaponReloading_OnReloaded;
            WeaponReloading.OnCanelReloaded += WeaponReloading_OnCanelReloaded;
            InventoryManager.Instance.OnSelectedWeaponDroped += Inventory_OnSelectedWeaponDroped;

            InventoryManager.Instance.OnGrenadeAdded += Inventory_OnGrenadeAmountChanged;
            InventoryManager.Instance.OnGrenadeSubstracted += Inventory_OnGrenadeSubstracted;

            Hide();
        }

        private void Inventory_OnGrenadeSubstracted(object sender, InventoryManager.OnGrenadeAmountChangedEventArgs e)
        {
            grenadeAmountIcon[e.grenadeAmount].SetActive(false);
        }

        private void Inventory_OnGrenadeAmountChanged(object sender, InventoryManager.OnGrenadeAmountChangedEventArgs e)
        {
            grenadeAmountIcon[e.grenadeAmount].SetActive(true);
        }

        private void Inventory_OnSelectedWeaponDroped(object sender, InventoryManager.OnSelectedWeaponChangedEventArgs e) => Hide();

        private void WeaponReloading_OnCanelReloaded(object sender, EventArgs e) => weaponReloadBar.Hide();

        private void WeaponReloading_OnReloaded(object sender, WeaponReloading.OnReloadedEventArgs e)
        {
            weaponReloadBar.ChangeFillAmountImmediately(e.reloadTime);
            weaponReloadBar.Show();
        }

        private void Inventory_OnSelectedWeaponChanged(object sender, EventArgs e)
        {
            InventoryManager inventory = sender as InventoryManager;
            if (inventory.UseWeapon == null)
            {
                Hide();
                return;
            }
            Show();
            weaponIcon.sprite = inventory.UseWeapon.WeaponSO.WeaponIcon;
            ChangeAmmoValueText(inventory);
        }

        private void Inventory_OnAmmoChanged(object sender, EventArgs e)
        {
            InventoryManager inventory = sender as InventoryManager;

            if (inventory.UseWeapon == null) return;

            ChangeAmmoValueText(inventory);
        }

        private void ChangeAmmoValueText(InventoryManager inventory) => ammoAmountText.SetText($"{inventory.UseWeapon.AmmoAmount} / {inventory.GetUseMagazine()}");

        private void Hide()
        {
            ammoAmountText.gameObject.SetActive(false);
            weaponIcon.gameObject.SetActive(false);
        } 
        private void Show()
        {
            ammoAmountText.gameObject.SetActive(true);
            weaponIcon.gameObject.SetActive(true);
        }
    }
}
