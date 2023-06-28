using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class CursorController: MonoBehaviour
    {

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
   
    }
}
