using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter
{
    public class ShootingMannequin:NetworkBehaviour, IDamageable
    {

        private const string ANIM_IS_SHOOT = "isShoot";

        private Animator animator;
        private WaitForSeconds waitForSeconds;

        [SerializeField] private float uprightingCooldown;
      
        private void Awake()
        {
            animator = GetComponent<Animator>();
            waitForSeconds = new WaitForSeconds(uprightingCooldown);
        }

        public bool TakeDamage(float damage, ulong clientId) 
        {
            TakeDamageServerRpc();
            return false;
        } 

        [ServerRpc(RequireOwnership = false)]
        private void TakeDamageServerRpc() => TakeDamageClientRpc();
      

        [ClientRpc()]
        private void TakeDamageClientRpc()
        {
            animator.SetBool(ANIM_IS_SHOOT, true);
            StartCoroutine(UprightingCoroutine());
        }

        private IEnumerator UprightingCoroutine()
        {
            yield return waitForSeconds;
            animator.SetBool(ANIM_IS_SHOOT, false);
        }

        public NetworkObject GetNetworkObject() => NetworkObject;

    }
}
