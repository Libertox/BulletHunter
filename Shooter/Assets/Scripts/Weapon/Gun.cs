using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Gun: MonoBehaviour, IInteractable
    {
        [SerializeField] private Rigidbody rgb;
        [field: SerializeField] public WeaponSO WeaponSO { get; private set; }
        [field: SerializeField] public int NumberOfMagazine { get; private set; }

        [field: SerializeField] public int AmmoAmount { get; private set; }


        public void Drop()
        {
            rgb.AddForce(Vector3.down, ForceMode.Impulse);
        }

        public void SetAmmoAmount(int ammoAmount) => AmmoAmount = ammoAmount;

        public void Interact(PlayerController playerController)
        {
            if (InventoryManager.Instance.AddWeapon(new WeaponInstance(WeaponSO, NumberOfMagazine, AmmoAmount)))
            {
                Destroy(gameObject);
            }
        }
    }
}
