using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float maxAngleY;
        [SerializeField] private float maxAngleX;

        private readonly float sensitivity = .5f;
        private float rotationY;


        private void Update()
        {
            CameraRotate();
        }

        private void CameraRotate()
        {
            rotationY += GameInput.Instance.GetMouseYAxis() * sensitivity;

            rotationY = Mathf.Clamp(rotationY, -maxAngleY, maxAngleY);

            transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        }

        
    }
}
