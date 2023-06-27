using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerAnimation:MonoBehaviour
    {
        private const string ANIM_IS_SQUAT = "isSquat";

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SquatAnimation(bool isSquat)
        {
            animator.SetBool(ANIM_IS_SQUAT, isSquat);
        }


    }
}
