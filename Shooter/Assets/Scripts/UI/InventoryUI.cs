using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Shooter.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI ammoAmountText;
        [SerializeField] private Image weaponIcon;
        [SerializeField] private BarUI weaponReloadBar;

        [SerializeField] private GameObject[] grenadeAmountIcon;

        private void Start()
        {
            Inventory.Instance.OnAmmoChanged += Inventory_OnAmmoChanged;
            Inventory.Instance.OnSelectedWeaponChanged += Inventory_OnSelectedWeaponChanged;
            Inventory.Instance.OnReloaded += Inventory_OnReloaded;
            Inventory.Instance.OnCanelReloaded += Inventory_OnCanelReloaded;
            Inventory.Instance.OnSelectedWeaponDroped += Inventory_OnSelectedWeaponDroped;

            Inventory.Instance.OnGrenadeAdded += Inventory_OnGrenadeAmountChanged;
            Inventory.Instance.OnGrenadeSubstracted += Inventory_OnGrenadeSubstracted;

            Hide();
        }

        private void Inventory_OnGrenadeSubstracted(object sender, Inventory.OnGrenadeAmountChangedEventArgs e)
        {
            grenadeAmountIcon[e.grenadeAmount].SetActive(false);
        }

        private void Inventory_OnGrenadeAmountChanged(object sender, Inventory.OnGrenadeAmountChangedEventArgs e)
        {
            grenadeAmountIcon[e.grenadeAmount].SetActive(true);
        }

        private void Inventory_OnSelectedWeaponDroped(object sender, Inventory.OnSelectedWeaponChangedEventArgs e) => Hide();

        private void Inventory_OnCanelReloaded(object sender, EventArgs e) => weaponReloadBar.Hide();

        private void Inventory_OnReloaded(object sender, Inventory.OnReloadedEventArgs e)
        {
            weaponReloadBar.ChangeFillAmount(e.reloadTime);
            weaponReloadBar.Show();
        }

        private void Inventory_OnSelectedWeaponChanged(object sender, EventArgs e)
        {
            Inventory inventory = sender as Inventory;
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
            Inventory inventory = sender as Inventory;

            if (inventory.UseWeapon == null) return;

            ChangeAmmoValueText(inventory);
        }

        private void ChangeAmmoValueText(Inventory inventory) => ammoAmountText.SetText($"{inventory.UseWeapon.AmmoAmount} / {inventory.GetUseMagazine()}");

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
