using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
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

        public void TakeDamage()
        {
            TakeDamageServerRpc();
        }

        private IEnumerator Uprighting()
        {
            yield return waitForSeconds;
            animator.SetBool(ANIM_IS_SHOOT, false);
        }

        [ServerRpc(RequireOwnership = false)]
        private void TakeDamageServerRpc()
        {
            TakeDamageClientRpc();
        }

        [ClientRpc()]
        private void TakeDamageClientRpc()
        {
            animator.SetBool(ANIM_IS_SHOOT, true);
            StartCoroutine(Uprighting());
        }

        public NetworkObject GetNetworkObject()
        {
            return NetworkObject;
        }
    }
}
