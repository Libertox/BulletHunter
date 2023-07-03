using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Weapon: MonoBehaviour, IInteractable
    {
        [SerializeField] private Rigidbody rgb;
        [SerializeField] private WeaponSO weaponSO;
        [SerializeField] private int numberOfMagazine;

        public WeaponSO WeaponSO => weaponSO;
        public int NumberOfMagazine => numberOfMagazine;

        public void Drop()
        {
            rgb.AddForce(Vector3.down, ForceMode.Impulse);
        }


        public void Interact(PlayerController playerController)
        {
            if(Inventory.Instance.AddWeapon(this))
            {
                Destroy(gameObject);
            }
        }
    }
}
