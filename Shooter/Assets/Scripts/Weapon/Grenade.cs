using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shooter
{
    public class Grenade: NetworkBehaviour
    {
        [SerializeField] private Rigidbody rgb;
        [SerializeField] private GameObject explosionParticleEffect;
        [SerializeField] private float rotationForce;
        [SerializeField] private float explosionTime;

        [field: SerializeField] public float ThrowForce { get; private set; }

        public float Mass => rgb.mass;
        public GameObject prefab;

        public void SetPrefab(GameObject gameObject) => prefab = gameObject;

        public void Throw(Vector3 direction)
        {
            rgb.velocity = Vector3.zero;
            rgb.angularVelocity = Vector3.zero;
            rgb.AddForce(direction * ThrowForce, ForceMode.Impulse);
            StartCoroutine(Explosion());
        }

        private IEnumerator Explosion()
        {
            yield return new WaitForSeconds(explosionTime);
            SpawnExplosionEffectServerRpc();
            NetworkObject.Despawn(false);
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnExplosionEffectServerRpc()
        {
            NetworkObject networkObject = NetworkObjectPool.Singleton.GetNetworkObject(explosionParticleEffect, transform.position, Quaternion.identity);
            networkObject.Spawn(true);
            networkObject.GetComponent<ParticleEffect>().Relase(explosionParticleEffect);
        }

    }
}
