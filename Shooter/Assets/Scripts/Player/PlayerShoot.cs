using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class PlayerShoot : NetworkBehaviour
    {
        public static event EventHandler<OnAnyPlayerKilledEventArgs> OnAnyPlayerKilled;

        public class OnAnyPlayerKilledEventArgs : EventArgs { public ulong targetId; }

        [SerializeField] private LayerMask shootLayerMask;

        [SerializeField] private GameObject bulletTrackPrefab;
        [SerializeField] private GameObject shootDustPrefab;

        private void Start()
        {
            if (IsOwner)
                GameInput.Instance.OnShooted += GameInput_OnShooted;
        }

        private void GameInput_OnShooted(object sender, EventArgs e)
        {
            if (!InventoryManager.Instance.CanShoot()) return;

            InventoryManager.Instance.UseWeapon.SubstractAmmo();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, InventoryManager.Instance.UseWeapon.WeaponSO.WeaponRange))
            {
                if (raycastHit.transform.TryGetComponent(out IDamageable damageable))
                {
                    HitObjectServerRpc(damageable.GetNetworkObject(), InventoryManager.Instance.UseWeapon.WeaponSO.Damage);
                }
                else
                {
                    SpawnShootEffectServerRpc(raycastHit.point.x, raycastHit.point.y, raycastHit.point.z);
                }
            }
            ShootServerRpc(transform.position.x, transform.position.y, transform.position.z);

        }

        [ServerRpc(RequireOwnership = false)]
        private void ShootServerRpc(float x, float y, float z) => ShootClientRpc(x, y, z);
       
        [ClientRpc]
        private void ShootClientRpc(float x, float y, float z) => SoundManager.Instance.PlayShootSound(new Vector3(x, y, z));
       

        [ServerRpc(RequireOwnership = false)]
        private void SpawnShootEffectServerRpc(float x, float y, float z)
        {
            SpawnEffect(new Vector3(x,y,z),bulletTrackPrefab);
            SpawnEffect(new Vector3(x, y, z), shootDustPrefab);
            SpawnShootEffectClientRpc(x, y, z);
        }

        [ClientRpc]
        private void SpawnShootEffectClientRpc(float x, float y, float z)
        {
            SoundManager.Instance.PlayBulletImpactSound(new Vector3(x, y, z));
        }

        private void SpawnEffect(Vector3 position, GameObject prefab)
        {
            NetworkObject networkObject = NetworkObjectPool.Singleton.GetNetworkObject(prefab,position, Quaternion.identity);
            networkObject.Spawn(true);
            networkObject.GetComponent<ParticleEffect>().Release(prefab);
        }

        [ServerRpc(RequireOwnership = false)]
        private void HitObjectServerRpc(NetworkObjectReference networkObjectReference, float damage)
        {
            HitObjectClientRpc(networkObjectReference, damage);
        }

        [ClientRpc()]
        private void HitObjectClientRpc(NetworkObjectReference networkObjectReference, float damage)
        {
            networkObjectReference.TryGet(out NetworkObject networkObject);
            IDamageable damageable = networkObject.GetComponent<IDamageable>();
            if(damageable.TakeDamage(damage, OwnerClientId))
            {
                if (IsOwner)
                {
                    OnAnyPlayerKilled?.Invoke(this, new OnAnyPlayerKilledEventArgs
                    {
                        targetId = networkObject.OwnerClientId
                    });
                }
            }
        }
    }


}
