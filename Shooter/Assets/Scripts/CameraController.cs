using System;
using UnityEngine;

namespace Shooter
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float maxAngleY;
        [SerializeField] private float unzoomValue;
        [SerializeField] private PlayerController playerController;

        private readonly float sensitivity = .5f;
        private float rotationY;
        private float rotationX;
        private Camera cameraToControl;

        private void Awake() => cameraToControl = GetComponent<Camera>();

        private void Start()
        {
            GameInput.Instance.OnAimed += GameInput_OnAimed;
            GameInput.Instance.OnCancelAimed += GameInput_OnCancelAimed;
            GameInput.Instance.OnWeaponDroped += GameInput_OnCancelAimed;
        }

        private void GameInput_OnCancelAimed(object sender, EventArgs e) => Unzoom();


        private void GameInput_OnAimed(object sender, EventArgs e)
        {
            if (InventoryManager.Instance.UseWeapon != null)
                Zoom();
        }

        private void Update() => Rotate();

        private void Rotate()
        {
            rotationY += GameInput.Instance.GetMouseYAxis() * sensitivity;
            rotationX += GameInput.Instance.GetMouseXAxis() * sensitivity;

            rotationY = Mathf.Clamp(rotationY, -maxAngleY, maxAngleY);
            transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
            playerController.HandleRotate(new Vector3(0, rotationX, 0));
        }

        private void Zoom() 
        {
            cameraToControl.fieldOfView = InventoryManager.Instance.UseWeapon.WeaponSO.WeaponZoom;
        }


        private void Unzoom() 
        {
            cameraToControl.fieldOfView = unzoomValue;
        } 

    }
}
