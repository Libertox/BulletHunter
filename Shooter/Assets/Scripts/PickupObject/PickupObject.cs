using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public abstract class PickupObject : NetworkBehaviour, IInteractable
    {
        protected PickupObjectSpawner pickupObjectSpawner;

        public abstract void Interact(PlayerController playerController);

        public void SetPickupObjectSpawner(PickupObjectSpawner pickupObjectSpawner) => this.pickupObjectSpawner = pickupObjectSpawner;

        public void Pickup()
        {
            gameObject.SetActive(false);
            PickupServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PickupServerRpc()
        {
            Destroy(gameObject);
            if(pickupObjectSpawner!= null) //Debug Check
            pickupObjectSpawner.RespawnPickupObject();
        }

           
    }
}
