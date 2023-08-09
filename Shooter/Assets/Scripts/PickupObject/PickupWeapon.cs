using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    internal class PickupWeapon : PickupObject
    {
        [SerializeField] private WeaponSO weaponSO;
        [SerializeField] private int magazineAmount;

        public override void Interact(PlayerController playerController)
        {
            if (InventoryManager.Instance.AddWeapon(new WeaponInstance(weaponSO,magazineAmount,0)))
                Pickup();
        }
    }
}
