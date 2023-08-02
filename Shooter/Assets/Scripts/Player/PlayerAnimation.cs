using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class PlayerAnimation:NetworkBehaviour
    {
        private const string ANIM_IS_SQUAT = "isSquat";
        private const string ANIM_IS_FALL = "isFall";
        private const string ANIM_IS_JUMP = "isJump";
        private const string ANIM_IS_SPRINT = "isSprint";
        private const string ANIM_IS_WALK = "isWalk";
        private const string ANIM_IS_DEATH = "isDeath";

        private Animator animator;

        private void Awake() => animator = GetComponent<Animator>();

     
        private void Start()
        {

            if (!IsOwner) return;

            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
                PlayerStats.Instance.OnRestored += PlayerStatsOnRestored;
            }
            else
            {
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;
            }

            PlayerController.OnSquated += PlayerController_OnSquated;

            PlayerController.OnWalked += PlayerController_OnWalked;
            PlayerController.OnSprinted += PlayerController_OnSprinted;
            PlayerController.OnJumped += PlayerController_OnJumped;
            PlayerController.OnFalled += PlayerController_OnFalled;
 
        }


        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;

                PlayerStats.Instance.OnRestored -= PlayerStatsOnRestored;
                PlayerStats.Instance.OnRestored += PlayerStatsOnRestored;
            }
        }

        private void PlayerStats_OnDeathed(object sender, EventArgs e)
        {
            animator.SetBool(ANIM_IS_DEATH, true);
        }
        private void PlayerStatsOnRestored(object sender, EventArgs e)
        {
            animator.SetBool(ANIM_IS_DEATH, false);
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
