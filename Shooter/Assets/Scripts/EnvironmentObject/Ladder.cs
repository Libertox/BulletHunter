using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Ladder : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerController playerController) => playerController.ClimbOnLadder();
    }
}
