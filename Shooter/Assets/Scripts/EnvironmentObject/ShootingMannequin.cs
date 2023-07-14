using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class ShootingMannequin:MonoBehaviour, IDamageable
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
            animator.SetBool(ANIM_IS_SHOOT, true);
            StartCoroutine(Uprighting());
        }

        private IEnumerator Uprighting()
        {
            yield return waitForSeconds;
            animator.SetBool(ANIM_IS_SHOOT, false);
        }



    }
}
