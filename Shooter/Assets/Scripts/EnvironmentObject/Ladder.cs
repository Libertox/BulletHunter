using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    public class Ladder : MonoBehaviour, IInteractable
    {
        public void Interact(PlayerController playerController) => playerController.ClimbOnLadder();
    }
}
