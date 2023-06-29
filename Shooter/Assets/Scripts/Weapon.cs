using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Weapon: MonoBehaviour, Interactable
    {
        [SerializeField] private Rigidbody rgb;
        [SerializeField] private WeaponSO weaponSO;
        [SerializeField] private int numberOfAmmo;

        public WeaponSO WeaponSO => weaponSO;
        public int NumberOfAmmo => numberOfAmmo;

        public void Drop()
        {
            rgb.AddForce(Vector3.down, ForceMode.Impulse);
        }


        public void Interact(PlayerController playerController)
        {
            if(playerController.Inventory.AddWeapon(this))
            {
                Destroy(gameObject);
            }
        }
    }
}
