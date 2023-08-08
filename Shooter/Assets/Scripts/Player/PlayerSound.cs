using System;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Shooter
{
    public class PlayerSound:MonoBehaviour
    {
        private bool playWalkSound;
        private float footstepTimer;
        private float footstepTimerMax = .25f;


        private void Start()
        {
            PlayerController.OnWalked += PlayerController_OnWalked;
        }

        private void PlayerController_OnWalked(object sender, PlayerController.OnStateChangedEventArgs e)
        {
            playWalkSound = e.state;
        }

        private void Update()
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer < 0f)
            {
                footstepTimer = footstepTimerMax;
                if (playWalkSound)
                {
                    
                    SoundManager.Instance.PlayPlayerWalkSound(transform.position);
                }
            }
        }
    }
}
