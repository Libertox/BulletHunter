using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class HealthPack : MonoBehaviour, IInteractable
    {
        [SerializeField] private float healValue;

        public void Interact(PlayerController playerController)
        {
            playerController.PlayerStats.IncreaseHealth(healValue);
            Destroy(gameObject);
        }
    }
}
