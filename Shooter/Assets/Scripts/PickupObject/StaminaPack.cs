using System;
using UnityEngine;

namespace BulletHaunter.PickupObject
{
    public class StaminaPack:PickupObject,IInteractable
    {
        [SerializeField] private float staminaValue;
        [SerializeField] private float bustDuration;

        public override void Interact(PlayerController playerController)
        {
            playerController.PlayerStats.IncreaseStamina(staminaValue);
            playerController.PlayerStats.SetStaminaBust(bustDuration);
            Pickup();
        }
    }
}
