using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class HealthPack : PickupObject, IInteractable
    {
        [SerializeField] private float healValue;

        public override void Interact(PlayerController playerController)
        {
            playerController.PlayerStats.IncreaseHealth(healValue);
            Pickup();
            
        }
    }
}
