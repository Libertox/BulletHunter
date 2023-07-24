using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class Gun: NetworkBehaviour, IInteractable
    {
        [SerializeField] private Rigidbody rgb;
        [SerializeField] private WeaponSO weaponSO;
        [SerializeField] private int numberOfMagazine;
        [SerializeField] private int ammoAmount;

        private float lifeTime = 48;

        public void Drop() => DropServerRpc();
      
        [ServerRpc(RequireOwnership = false)]
        private void DropServerRpc() => DropClientRpc();
      
        [ClientRpc()]
        private void DropClientRpc()
        {
            rgb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
            Destroy(gameObject, lifeTime);
        }

        public void SetAmmoAmount(int ammoAmount) => SetAmmoAmountClientRpc(ammoAmount);

        [ClientRpc]
        private void SetAmmoAmountClientRpc(int ammoAmount) => this.ammoAmount = ammoAmount;


        public void Interact(PlayerController playerController)
        {
            if (InventoryManager.Instance.AddWeapon(new WeaponInstance(weaponSO, numberOfMagazine, ammoAmount)))
                DestroySelfServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroySelfServerRpc() => DestroySelfClientRpc();
      

        [ClientRpc()]
        private void DestroySelfClientRpc() => Destroy(gameObject);
      

    }
}
