﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PickupGrenade : PickupObject, IInteractable
    {
        public override void Interact(PlayerController playerController)
        {
            if (Inventory.Instance.AddOneGranade())
            {
                pickupObjectSpawner.SpawnNewObject();
                gameObject.SetActive(false);
            }
                
        }
    }
}
