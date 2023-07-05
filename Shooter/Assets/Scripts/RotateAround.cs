using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class RotateAround: MonoBehaviour
    {
        [SerializeField] private Vector3 angleToRotate;
        [SerializeField] private float rotationSpeed;
       
        private void Update() => transform.eulerAngles += (angleToRotate * (Time.deltaTime * rotationSpeed));
     
    }
}
