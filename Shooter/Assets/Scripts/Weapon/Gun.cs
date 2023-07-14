using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Gun: MonoBehaviour, IInteractable
    {
        [SerializeField] private Rigidbody rgb;
        [SerializeField] private WeaponSO weaponSO;
        [SerializeField] private int numberOfMagazine;
        [SerializeField] private int ammoAmount;

        private float lifeTime = 48;

        public void Drop()
        {
            rgb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
            Destroy(gameObject, lifeTime);
        }

        public void SetAmmoAmount(int ammoAmount) => this.ammoAmount = ammoAmount;

        public void Interact(PlayerController playerController)
        {
            if (InventoryManager.Instance.AddWeapon(new WeaponInstance(weaponSO, numberOfMagazine, ammoAmount)))
            {
                Destroy(gameObject);
            }
        }

    }
}
