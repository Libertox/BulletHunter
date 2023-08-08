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
        [SerializeField] private float baseDamage;
        [SerializeField] private float explosionRange;

        [SerializeField] private LayerMask targerLayerMask;

        private readonly float maxDistance = 3f;
        private GameObject prefab;

        [field: SerializeField] public float ThrowForce { get; private set; }
        public float Mass => rgb.mass;

        public ulong playerid;

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
            PlayExplosionSoundServerRpc(transform.position.x, transform.position.y, transform.position.z);
            HurtAllTargetServerRpc();
            NetworkObject.Despawn(false);
            NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnExplosionEffectServerRpc()
        {
            NetworkObject networkObject = NetworkObjectPool.Singleton.GetNetworkObject(explosionParticleEffect, transform.position, Quaternion.identity);
            networkObject.Spawn(true);
            networkObject.GetComponent<ParticleEffect>().Release(explosionParticleEffect);
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayExplosionSoundServerRpc(float x, float y, float z)
        {
            PlayExplosionSoundClientRpc(x, y, z);
        }

        [ClientRpc]
        private void PlayExplosionSoundClientRpc(float x, float y, float z)
        {
            SoundManager.Instance.PlayGrenadeExplosionSound(new Vector3(x, y, z));
        }


        [ServerRpc(RequireOwnership = false)]
        private void HurtAllTargetServerRpc() => HurtAllTargetClientRpc();
       
        [ClientRpc]
        private void HurtAllTargetClientRpc()
        {
            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, explosionRange, Vector3.up, maxDistance, targerLayerMask);
            foreach (RaycastHit hit in raycastHits)
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    float distanceFromExplosionCenter = Vector3.Distance(hit.transform.position, transform.position);
                    float damage = explosionRange  / distanceFromExplosionCenter + baseDamage;
                    damageable.TakeDamage(damage, playerid);
                }
            }
        }
    }
}
