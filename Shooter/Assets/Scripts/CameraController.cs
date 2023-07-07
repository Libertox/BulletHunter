using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float maxAngleY;
        [SerializeField] private float unzoomValue;

        private readonly float sensitivity = .5f;
        private float rotationY;
        private Camera cameraToControl;
        float rotationX;
        private void Awake()
        {
            cameraToControl = GetComponent<Camera>();
        }

        private void Start()
        {
            GameInput.Instance.OnAimed += GameInput_OnAimed;
            GameInput.Instance.OnCancelAimed += GameInput_OnCancelAimed;
            GameInput.Instance.OnWeaponDroped += GameInput_OnCancelAimed;
        }

        private void GameInput_OnCancelAimed(object sender, EventArgs e)
        {
            Unzoom();
        }

        private void GameInput_OnAimed(object sender, EventArgs e)
        {
            if (Inventory.Instance.UseWeapon)
                Zoom();
        }
       

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            rotationY += GameInput.Instance.GetMouseYAxis() * sensitivity;
            rotationX += GameInput.Instance.GetMouseXAxis();
            rotationY = Mathf.Clamp(rotationY, -maxAngleY, maxAngleY);
            //rotationX = Mathf.Clamp(rotationX, -360, 360);
            transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
        }

        private void Zoom()
        {
            cameraToControl.fieldOfView = Inventory.Instance.UseWeapon.WeaponZoom;
        }

        private void Unzoom()
        {
            cameraToControl.fieldOfView = unzoomValue;
        }
        
    }
}
