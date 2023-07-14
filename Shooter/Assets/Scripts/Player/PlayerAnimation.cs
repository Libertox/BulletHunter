﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerAnimation:MonoBehaviour
    {
        private const string ANIM_IS_SQUAT = "isSquat";
        private const string ANIM_IS_FALL = "isFall";
        private const string ANIM_IS_JUMP = "isJump";
        private const string ANIM_IS_SPRINT = "isSprint";
        private const string ANIM_IS_WALK = "isWalk";

        private Animator animator;

        private void Awake() => animator = GetComponent<Animator>();

        private void Start()
        {
            PlayerController.OnSquated += PlayerController_OnSquated;

            PlayerController.OnWalked += PlayerController_OnWalked;
            PlayerController.OnSprinted += PlayerController_OnSprinted;
            PlayerController.OnJumped += PlayerController_OnJumped;
            PlayerController.OnFalled += PlayerController_OnFalled;
        }

        private void PlayerController_OnFalled(object sender, PlayerController.OnStateChangedEventArgs e)
        {
            animator.SetBool(ANIM_IS_FALL, e.state);
        }

        private void PlayerController_OnJumped(object sender, PlayerController.OnStateChangedEventArgs e)
        {
            animator.SetBool(ANIM_IS_JUMP, e.state);
        }

        private void PlayerController_OnSprinted(object sender, PlayerController.OnSprintedEventArgs e)
        {
            animator.SetBool(ANIM_IS_SPRINT, e.isSprint);
        }

        private void PlayerController_OnWalked(object sender, PlayerController.OnStateChangedEventArgs e)
        {
            animator.SetBool(ANIM_IS_WALK, e.state);
        }

        private void PlayerController_OnSquated(object sender, PlayerController.OnStateChangedEventArgs e)
        {
            animator.SetBool(ANIM_IS_SQUAT, e.state);
        }
    }
}