using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class PlayerShoot:MonoBehaviour
    {
        [SerializeField] private LayerMask damageableLayerMask;

        [SerializeField] private Transform shootEffectTransform;

        private void Start()
        {
            GameInput.Instance.OnShooted += GameInput_OnShooted;
        }

        private void GameInput_OnShooted(object sender, EventArgs e)
        {
            if (!InventoryManager.Instance.CanShoot()) return;

            InventoryManager.Instance.UseWeapon.SubstractAmmo();


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, InventoryManager.Instance.UseWeapon.WeaponSO.WeaponRange))
            {
                if (raycastHit.transform.TryGetComponent(out IDamageable damageable))
                {
                    HitObjectServerRpc(damageable.GetNetworkObject());
                }
                else
                {
                    ObjectPoolingManager.Instance.BulletTrackPool.Get().Init(raycastHit.point, ObjectPoolingManager.Instance.BulletTrackPool);
                    ObjectPoolingManager.Instance.ShootEffectPool.Get().Init(raycastHit.point, ObjectPoolingManager.Instance.ShootEffectPool);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void HitObjectServerRpc(NetworkObjectReference networkObjectReference)
        {
            HitObjectClientRpc(networkObjectReference);
        }

        [ClientRpc()]
        private void HitObjectClientRpc(NetworkObjectReference networkObjectReference)
        {
            networkObjectReference.TryGet(out NetworkObject networkObject);
            IDamageable damageable = networkObject.GetComponent<IDamageable>();
            damageable.TakeDamage();
        }
    }


}
