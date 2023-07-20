using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PickupAmmoPack:PickupObject, IInteractable
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private int amount;

        public override void Interact(PlayerController playerController)
        {
            InventoryManager.Instance.AddMagazine(weaponType, amount);
            Pickup();
        }

    }
}
