using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public interface IInteractable
    {
        public void Interact(PlayerController playerController);

    }
}
