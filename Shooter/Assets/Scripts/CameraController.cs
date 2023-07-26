using System;
using Unity.Netcode;
using UnityEngine;

namespace Shooter
{
    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private float maxAngleY;
        [SerializeField] private float unzoomValue;
        [SerializeField] private PlayerController playerController;

        private float rotationY;
        private float rotationX;
        private Camera cameraToControl;

        private void Awake() 
        {
            cameraToControl = GetComponent<Camera>();
        }

        private void Start()
        {
            if (!IsOwner) 
            {
                Hide();
                return;
            }

            GameInput.Instance.OnAimed += GameInput_OnAimed;
            GameInput.Instance.OnCancelAimed += GameInput_OnCancelAimed;
            GameInput.Instance.OnWeaponDroped += GameInput_OnCancelAimed;

            if(PlayerStats.Instnace != null)
            {
                PlayerStats.Instnace.OnDeathed += PlayerStats_OnDeathed;
                PlayerStats.Instnace.OnRestored += PlayerStats_OnRestored;
            }   
            else
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;

        }

        private void PlayerStats_OnRestored(object sender, EventArgs e)
        {
            Show();
        }

        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instnace != null)
            {
                PlayerStats.Instnace.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instnace.OnDeathed += PlayerStats_OnDeathed;

                PlayerStats.Instnace.OnRestored -= PlayerStats_OnRestored;
                PlayerStats.Instnace.OnRestored += PlayerStats_OnRestored;
            }
                
        }

        private void PlayerStats_OnDeathed(object sender, EventArgs e) => Hide();
        
        private void GameInput_OnCancelAimed(object sender, EventArgs e) => Unzoom();

        private void GameInput_OnAimed(object sender, EventArgs e)
        {
            if (InventoryManager.Instance.UseWeapon != null)
                Zoom();
        }

        private void Update() => Rotate();
     
        private void Rotate()
        {
            rotationY += GameInput.Instance.GetMouseYAxis() * Time.deltaTime;
            rotationX += GameInput.Instance.GetMouseXAxis() * Time.deltaTime;

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

        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);

    }
}
