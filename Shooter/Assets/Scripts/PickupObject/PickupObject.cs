using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public abstract class PickupObject : MonoBehaviour, IInteractable
    {
        [SerializeField] protected PickupObjectSpawner pickupObjectSpawner;

        public abstract void Interact(PlayerController playerController);
        
    }
}
