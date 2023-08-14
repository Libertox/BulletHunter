using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    public class CursorController: MonoBehaviour
    {
        private void Start() => Cursor.lockState = CursorLockMode.Locked;
      
    }
}
