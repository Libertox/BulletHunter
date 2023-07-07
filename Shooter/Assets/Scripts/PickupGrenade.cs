using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PickupGrenade : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerController playerController)
        {
            if (Inventory.Instance.AddOneGranade())
                Destroy(gameObject);
        }
    }
}
