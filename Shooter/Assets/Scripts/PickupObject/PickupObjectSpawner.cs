using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class PickupObjectSpawner:NetworkBehaviour
    {
        [SerializeField] private float spawnDuration;
        [SerializeField] private PickupObject[] spawnObjects;

        private WaitForSeconds waitForSeconds;

        private void Start() 
        {
            waitForSeconds = new WaitForSeconds(spawnDuration);
          
        }

        public override void OnNetworkSpawn()
        {
            if(IsServer)
                SpawnServerRpc();
        }


        [ServerRpc(RequireOwnership = false)]
        private void SpawnServerRpc()
        {
            PickupObject pickupObject = Instantiate(GetNewRandomPickupObject());
            pickupObject.transform.position = transform.position;
            pickupObject.SetPickupObjectSpawner(this);

            NetworkObject networkObject = pickupObject.GetComponent<NetworkObject>();
            networkObject.Spawn(true);

        }

 
        private PickupObject GetNewRandomPickupObject()
        {
            int pickupObjectIndex = UnityEngine.Random.Range(0, spawnObjects.Length);
            return spawnObjects[pickupObjectIndex];
        }

        public void SpawnNewObject() => SpawnNewObjectServerRpc();

        [ServerRpc(RequireOwnership = false)]
        private void SpawnNewObjectServerRpc()
        {
            StartCoroutine(WaitForSpawnNewObject());
        }

        private IEnumerator WaitForSpawnNewObject()
        {
            yield return waitForSeconds;
            SpawnServerRpc();
        }

    }
}
