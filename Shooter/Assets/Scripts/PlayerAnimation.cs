using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerAnimation:MonoBehaviour
    {
        private const string ANIM_IS_SQUAT = "isSquat";

        private Animator animator;

        private void Awake() => animator = GetComponent<Animator>();

        private void Start()
        {
            PlayerController.OnSquated += PlayerController_OnSquated;
        }

        private void PlayerController_OnSquated(object sender, PlayerController.OnSquatedEventArgs e)
        {
            animator.SetBool(ANIM_IS_SQUAT, e.isSquat);
        }



    }
}
