﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class Gun: NetworkBehaviour, IInteractable
    {
        [SerializeField] private Rigidbody rgb;
        [SerializeField] private WeaponSO weaponSO;
        [SerializeField] private int numberOfMagazine;
        [SerializeField] private int ammoAmount;

        private readonly float lifeTime = 48;

        private void Start() => GameManager.Instance.OnPlayerReconnected += GameManager_OnPlayerReconnected;
        
        private void GameManager_OnPlayerReconnected(object sender, EventArgs e) => SetAmmoAmount(ammoAmount);
       
        public override void OnDestroy() => GameManager.Instance.OnPlayerReconnected -= GameManager_OnPlayerReconnected;

       
        public void Drop() => DropServerRpc();
      
        [ServerRpc(RequireOwnership = false)]
        private void DropServerRpc() => DropClientRpc();
      
        [ClientRpc()]
        private void DropClientRpc()
        {
            float dropForce = 2f;
            rgb.AddForce(Vector3.down * dropForce, ForceMode.Impulse);
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
