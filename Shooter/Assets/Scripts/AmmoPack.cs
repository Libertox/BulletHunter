using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class AmmoPack:MonoBehaviour, Interactable
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private int amount;


        public void Interact(PlayerController playerController)
        {
            playerController.Inventory.AddAmmo(weaponType, amount);
            Destroy(gameObject);
        }

    }
}
