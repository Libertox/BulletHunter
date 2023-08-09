using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    public class ArmorPack:PickupObject,IInteractable
    {
        [SerializeField] private float armorValue;

        public override void Interact(PlayerController playerController)
        {
            playerController.PlayerStats.IncreaseArmor(armorValue);
            Pickup();
        }
    }
}
