using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Weapon: MonoBehaviour, IInteractable
    {
        [SerializeField] private Rigidbody rgb;
        [field: SerializeField] public WeaponSO WeaponSO { get; private set; }
        [field: SerializeField] public int NumberOfMagazine { get; private set; }


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
