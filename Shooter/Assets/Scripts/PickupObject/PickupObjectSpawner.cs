using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter.PickupObject
{
    public class PickupObjectSpawner:NetworkBehaviour
    {
        [SerializeField] private float spawnDuration;
        [SerializeField] private GameObject[] spawnObjectsPrefabs;

        private WaitForSeconds waitForSeconds;

        private void Awake() => waitForSeconds = new WaitForSeconds(spawnDuration);

        private void Start()
        {
            if(IsServer)
                SpawnPickupObjectServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnPickupObjectServerRpc()
        {
            GameObject prefab = GetRandomPickupObject();
            NetworkObject networkObject = NetworkObjectPool.Singleton.GetNetworkObject(prefab, transform.position, Quaternion.identity);
            PickupObject pickupObject = networkObject.GetComponent<PickupObject>();
            pickupObject.SetPickupObjectSpawner(this);
            pickupObject.SetPickupObjectPrefab(prefab);
            networkObject.Spawn(true);
        }

        private GameObject GetRandomPickupObject()
        {
            int pickupObjectIndex = UnityEngine.Random.Range(0, spawnObjectsPrefabs.Length);
            return spawnObjectsPrefabs[pickupObjectIndex];
        }

        public void RespawnPickupObject() => RespawnPickupObjectServerRpc();

        [ServerRpc(RequireOwnership = false)]
        private void RespawnPickupObjectServerRpc() => StartCoroutine(WaitForSpawnPickupObject());
        
        private IEnumerator WaitForSpawnPickupObject()
        {
            yield return waitForSeconds;
            SpawnPickupObjectServerRpc();
        }

    }
}
