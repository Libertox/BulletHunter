using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHaunter
{
    public class CursorController: MonoBehaviour
    {

        private void Start() 
        {
            Cursor.lockState = CursorLockMode.Locked;    
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
