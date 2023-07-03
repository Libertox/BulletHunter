using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerShoot:MonoBehaviour
    {
        [SerializeField] private LayerMask damageableLayerMask;

        private void Start()
        {
            GameInput.Instance.OnShooted += GameInput_OnShooted;
        }

        private void GameInput_OnShooted(object sender, EventArgs e)
        {
            if (!Inventory.Instance.CanShoot()) return;

            Inventory.Instance.SubstractAmmo();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if(Physics.Raycast(ray, out raycastHit, Inventory.Instance.UseWeapon.WeaponRange, damageableLayerMask))
            {
                if(raycastHit.transform.TryGetComponent(out IDamageable damageable))
                {                
                    damageable.TakeDamage();
                }
            }
        }


    }


}
