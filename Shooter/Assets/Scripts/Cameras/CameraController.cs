using System;
using Unity.Netcode;
using UnityEngine;

namespace BulletHaunter.Cameras
{
    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private float maxAngleY;
        [SerializeField] private float unzoomValue;
        [SerializeField] private float movementSpeed;

        [SerializeField] private PlayerController playerController;

        [SerializeField] private Transform uprightCameraPosition;
        [SerializeField] private Transform squatCameraPosition;

        private float rotationY;
        private float rotationX;
        private Camera cameraToControl;

        private bool isSquatPosition;

        private void Awake() => cameraToControl = GetComponent<Camera>();
        
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

            PlayerController.OnSquated += PlayerController_OnSquated;

            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;
                PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;
            }
            else
                PlayerStats.OnAnyPlayerSpawn += PlayerStats_OnAnyPlayerSpawn;

            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameManagerMultiplayer_OnPlayerDataNetworkListChanged;

            SetCullingMask();
        }

        private void PlayerController_OnSquated(object sender, PlayerController.OnStateChangedEventArgs e)
        {
            isSquatPosition = e.state;
        }

        private void GameManagerMultiplayer_OnPlayerDataNetworkListChanged(object sender, EventArgs e)
        {
            ResetCullingMask();
            SetCullingMask();
        }

        public override void OnDestroy()
        {
            GameManagerMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameManagerMultiplayer_OnPlayerDataNetworkListChanged;
        }

        private void SetCullingMask()
        {
            int index = GameManagerMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId);

            if (index == -1) 
                return;

            LayerMask playerMask = GameManager.Instance.GetPlayerLayerMask(index);
            LayerMask gunLayerMask = GameManager.Instance.GetPlayerGunLayerMask(index);
            cameraToControl.cullingMask &= ~(1 << playerMask);
            cameraToControl.cullingMask &= ~(1 << gunLayerMask);
        }
        private void ResetCullingMask()
        {
            for (int i = 0; i < GameManager.Instance.GetPlayerGunLayerMaskLength(); i++)
            {
                cameraToControl.cullingMask |= (1 << GameManager.Instance.GetPlayerGunLayerMask(i));
                cameraToControl.cullingMask |= (1 << GameManager.Instance.GetPlayerLayerMask(i));
            }
        }

        private void PlayerStats_OnRestored(object sender, EventArgs e) => Show();
       
        private void PlayerStats_OnAnyPlayerSpawn(object sender, EventArgs e)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.OnDeathed -= PlayerStats_OnDeathed;
                PlayerStats.Instance.OnDeathed += PlayerStats_OnDeathed;

                PlayerStats.Instance.OnRestored -= PlayerStats_OnRestored;
                PlayerStats.Instance.OnRestored += PlayerStats_OnRestored;
            }
                
        }

        private void PlayerStats_OnDeathed(object sender, EventArgs e) => Hide();
        
        private void GameInput_OnCancelAimed(object sender, EventArgs e) => Unzoom();

        private void GameInput_OnAimed(object sender, EventArgs e)
        {
            if (InventoryManager.Instance.UseWeapon != null)
                Zoom();
        }

        private void Update() 
        {
            Rotate();

            if (isSquatPosition)
                transform.position = Vector3.LerpUnclamped(transform.position, squatCameraPosition.position, Time.deltaTime * movementSpeed);
            else
                transform.position = Vector3.LerpUnclamped(transform.position, uprightCameraPosition.position, Time.deltaTime * movementSpeed);
        } 
     
        private void Rotate()
        {
            rotationY += GameInput.Instance.GetMouseYAxis() * Time.deltaTime;
            rotationX += GameInput.Instance.GetMouseXAxis() * Time.deltaTime;

            rotationY = Mathf.Clamp(rotationY, -maxAngleY, maxAngleY);
            transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
            playerController.HandleRotate(new Vector3(0, rotationX, 0));
        }

        private void Zoom() => cameraToControl.fieldOfView = InventoryManager.Instance.UseWeapon.WeaponSO.WeaponZoom;
      
        private void Unzoom() => cameraToControl.fieldOfView = unzoomValue;
      
        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);

    }
}
