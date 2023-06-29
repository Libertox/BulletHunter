using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Shooter
{
    public class GameInput: MonoBehaviour
    {
        public static GameInput Instance { get; private set; }

        public event EventHandler OnJumped;
        public event EventHandler OnSquat;
        public event EventHandler OnInteract;
        public event EventHandler OnWeaponDroped;
        public event EventHandler<OnWeaponSelectedEventArgs> OnWeaponSelected;

        public class OnWeaponSelectedEventArgs: EventArgs { public int selectWeaponIndex; }

        private PlayerInput playerInput;
        private OnWeaponSelectedEventArgs selectedWeapon;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            playerInput = new PlayerInput();
            selectedWeapon = new OnWeaponSelectedEventArgs();

            playerInput.Enable();

            playerInput.Player.Jump.performed += Jump_performed;
            playerInput.Player.Squat.performed += Squat_performed;
            playerInput.Player.Interact.performed += Interact_performed;
            playerInput.Player.SelectWeaponByScroll.performed += SelectWeaponByScroll_performed;
            playerInput.Player.SelectWeapon.performed += SelectWeapon_performed;
            playerInput.Player.DropWeapon.performed += DropWeapon_performed;

        }

        private void DropWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnWeaponDroped?.Invoke(this, EventArgs.Empty);
     
        private void SelectWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            for (int i = 0; i < playerInput.Player.SelectWeapon.bindings.Count; i++)
            {
                if (playerInput.Player.SelectWeapon.bindings[i].ToDisplayString() == obj.action.activeControl.displayName)
                {
                    selectedWeapon.selectWeaponIndex = i;
                    break;
                }
            }
            OnWeaponSelected?.Invoke(this, selectedWeapon);
        }

        private void SelectWeaponByScroll_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            var readValue = obj.ReadValue<float>();
            int inventorySize = Inventory.MaxNumberWeapon - 1;
            if(readValue > 0)
            {
                if(selectedWeapon.selectWeaponIndex < inventorySize)
                    selectedWeapon.selectWeaponIndex++;

                else if(selectedWeapon.selectWeaponIndex == inventorySize)
                    selectedWeapon.selectWeaponIndex = 0;
            }
            else if(readValue < 0)
            {
                if (selectedWeapon.selectWeaponIndex > 0)
                    selectedWeapon.selectWeaponIndex--;

                else if(selectedWeapon.selectWeaponIndex == 0)
                    selectedWeapon.selectWeaponIndex = inventorySize;
            }
            OnWeaponSelected?.Invoke(this, selectedWeapon);
        }

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnInteract?.Invoke(this, EventArgs.Empty);
        
        private void Squat_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnSquat?.Invoke(this, EventArgs.Empty);
      
        private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) => OnJumped?.Invoke(this, EventArgs.Empty);
       
        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 movment = playerInput.Player.Move.ReadValue<Vector2>();
            return movment.normalized;

        }

        public float GetSprintValue() => playerInput.Player.Sprint.ReadValue<float>();

        public float GetMouseXAxis() => playerInput.Player.CameraRotationX.ReadValue<float>();
      
        public float GetMouseYAxis() => playerInput.Player.CameraRotationY.ReadValue<float>();
       
    }
}
