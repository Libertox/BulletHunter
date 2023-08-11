using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter.PickupObject
{
    public class PickupGrenade : PickupObject, IInteractable
    {
        public override void Interact(PlayerController playerController)
        {
            if (InventoryManager.Instance.AddOneGranade())
                Pickup();         
        }
    }
}
