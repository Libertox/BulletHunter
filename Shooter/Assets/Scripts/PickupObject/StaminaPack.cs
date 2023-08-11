﻿using System;
using UnityEngine;

namespace BulletHaunter.PickupObject
{
    public class StaminaPack:PickupObject,IInteractable
    {
        [SerializeField] private float staminaValue;

        public override void Interact(PlayerController playerController)
        {
            playerController.PlayerStats.IncreaseStamina(staminaValue);
            playerController.PlayerStats.SetStaminaBust();
            Pickup();
        }
    }
}
