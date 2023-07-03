using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class AmmoPack:MonoBehaviour, IInteractable
    {
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private int amount;


        public void Interact(PlayerController playerController)
        {
            Inventory.Instance.AddMagazine(weaponType, amount);
            Destroy(gameObject);
        }

    }
}
