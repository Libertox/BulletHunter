using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class AmmoPack:PickupObject, IInteractable
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private int amount;

      
        public override void Interact(PlayerController playerController)
        {
            Inventory.Instance.AddMagazine(weaponType, amount);
            pickupObjectSpawner.SpawnNewObject();
            gameObject.SetActive(false);
        }

    }
}
