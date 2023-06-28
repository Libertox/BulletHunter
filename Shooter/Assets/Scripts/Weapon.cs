using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class Weapon: MonoBehaviour
    {
        [SerializeField] private Rigidbody rgb;


        public void Drop()
        {
            rgb.AddForce(Vector3.down, ForceMode.Impulse);
        }
    }
}
