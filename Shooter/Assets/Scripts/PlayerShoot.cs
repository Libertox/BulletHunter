using System;
using System.Collections.Generic;
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
            if (!Inventory.Instance.CanShoot()) return;

            Inventory.Instance.SubstractAmmo();


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, Inventory.Instance.UseWeapon.WeaponRange))
            {
                if (raycastHit.transform.TryGetComponent(out IDamageable damageable))
                {                
                    damageable.TakeDamage();
                }
                else
                {
                    ObjectPoolingManager.Instance.BulletTrackPool.Get().Init(raycastHit.point, ObjectPoolingManager.Instance.BulletTrackPool);
                    ObjectPoolingManager.Instance.ShootEffectPool.Get().Init(raycastHit.point, ObjectPoolingManager.Instance.ShootEffectPool);
                }
            }
        }


    }


}
