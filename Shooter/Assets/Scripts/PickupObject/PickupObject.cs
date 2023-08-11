using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter.PickupObject
{
    public abstract class PickupObject : NetworkBehaviour, IInteractable
    {
        protected PickupObjectSpawner pickupObjectSpawner;

        private GameObject pickupObjetcPrefab;

        public abstract void Interact(PlayerController playerController);

        public void SetPickupObjectSpawner(PickupObjectSpawner pickupObjectSpawner) => this.pickupObjectSpawner = pickupObjectSpawner;

        public void SetPickupObjectPrefab(GameObject prefab) => pickupObjetcPrefab = prefab;

        public void Pickup()
        {
            gameObject.SetActive(false);
            SoundManager.Instance.PlayPickupObjectSound(transform.position);
            PickupServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PickupServerRpc()
        {
            NetworkObject.Despawn(false);
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, pickupObjetcPrefab);
            pickupObjectSpawner.RespawnPickupObject();
        }

           
    }
}
