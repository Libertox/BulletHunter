using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shooter
{
    public class GameInput: MonoBehaviour
    {
        public static GameInput Instance { get; private set; }

        private const string PLAYER_PREFS_MOUSE_SENSITIVITY = "MouseSensitivity";
        private const string PLAYER_PREFS_BINDING = "Binding";

        public event EventHandler OnJumped;
        public event EventHandler OnSquat;
        public event EventHandler OnInteract;
        public event EventHandler OnWeaponDroped;
        public event EventHandler OnShooted;
        public event EventHandler OnReloaded;

        public event EventHandler OnAimed;
        public event EventHandler OnCancelAimed;

        public event EventHandler OnThrowed;
        public event EventHandler OnCancelThrowed;

        public event EventHandler OnPaused;

        public event EventHandler<OnWeaponSelectedEventArgs> OnWeaponSelected;

        public class OnWeaponSelectedEventArgs: EventArgs { public int selectWeaponIndex; }

        private PlayerInput playerInput;
        private OnWeaponSelectedEventArgs selectedWeapon;
        

        public float MouseSensitivity { get; private set; }
        private readonly float defaultMouseSensitivity = 50f;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            playerInput = new PlayerInput();
            selectedWeapon = new OnWeaponSelectedEventArgs();

            MouseSensitivity = PlayerPrefs.GetFloat(PLAYER_PREFS_MOUSE_SENSITIVITY, defaultMouseSensitivity);

            if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING))
                playerInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING));

            playerInput.Enable();

            playerInput.Player.Jump.performed += Jump_performed;
            playerInput.Player.Squat.performed += Squat_performed;
            playerInput.Player.Interact.performed += Interact_performed;
            playerInput.Player.SelectWeaponByScroll.performed += SelectWeaponByScroll_performed;
            playerInput.Player.SelectWeapon.performed += SelectWeapon_performed;
            playerInput.Player.DropWeapon.performed += DropWeapon_performed;
            playerInput.Player.Shoot.performed += Shoot_performed;
            playerInput.Player.Reload.performed += Reload_performed;

            playerInput.Player.Aim.performed += Aim_performed;
            playerInput.Player.Aim.canceled += Aim_canceled;

            playerInput.Player.Throw.performed += Throw_performed;
            playerInput.Player.Throw.canceled += Throw_canceled;

            playerInput.UI.Pause.performed += Pause_performed;

        }

        private void OnDestroy()
        {
            playerInput.Player.Jump.performed -= Jump_performed;
            playerInput.Player.Squat.performed -= Squat_performed;
            playerInput.Player.Interact.performed -= Interact_performed;
            playerInput.Player.SelectWeaponByScroll.performed -= SelectWeaponByScroll_performed;
            playerInput.Player.SelectWeapon.performed -= SelectWeapon_performed;
            playerInput.Player.DropWeapon.performed -= DropWeapon_performed;
            playerInput.Player.Shoot.performed -= Shoot_performed;
            playerInput.Player.Reload.performed -= Reload_performed;

            playerInput.Player.Aim.performed -= Aim_performed;
            playerInput.Player.Aim.canceled -= Aim_canceled;

            playerInput.Player.Throw.performed -= Throw_performed;
            playerInput.Player.Throw.canceled -= Throw_canceled;

            playerInput.UI.Pause.performed -= Pause_performed;

            playerInput.Dispose();
        }

        private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnPaused?.Invoke(this, EventArgs.Empty);
        }

        private void Throw_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if(GameManager.Instance!= null && !GameManager.Instance.IsPlayState()) return;

            OnCancelThrowed?.Invoke(this, EventArgs.Empty);
        }

        private void Throw_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            OnThrowed?.Invoke(this, EventArgs.Empty);
        }

        private void Aim_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            OnCancelAimed?.Invoke(this, EventArgs.Empty);
        }

        private void Aim_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            OnAimed?.Invoke(this, EventArgs.Empty);
        }

        private void Reload_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) 
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            OnReloaded?.Invoke(this, EventArgs.Empty);
        }

        private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) 
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            OnShooted?.Invoke(this, EventArgs.Empty);
        }

        private void DropWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            OnWeaponDroped?.Invoke(this, EventArgs.Empty);
        }
        private void SelectWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

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
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            var readValue = obj.ReadValue<float>();
            int inventorySize = InventoryManager.MaxNumberWeapon - 1;
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

        private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            OnInteract?.Invoke(this, EventArgs.Empty);
        }
        private void Squat_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            OnSquat?.Invoke(this, EventArgs.Empty);
        }
        private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsPlayState()) return;

            OnJumped?.Invoke(this, EventArgs.Empty);
        }
        public Vector2 GetMovementVectorNormalized()
        {
            if (!GameManager.Instance.IsPlayState()) return Vector2.zero;

            Vector2 movment = playerInput.Player.Move.ReadValue<Vector2>();
            return movment.normalized;

        }

        public float GetSprintValue() 
        {
            if (!GameManager.Instance.IsPlayState()) return 0;
            return playerInput.Player.Sprint.ReadValue<float>();
        }

        public float GetMouseXAxis() 
        {
            if (!GameManager.Instance.IsPlayState()) return 0;
            return playerInput.Player.CameraRotationX.ReadValue<float>() * MouseSensitivity;
        }

        public float GetMouseYAxis() 
        {
            if (!GameManager.Instance.IsPlayState()) return 0;
            return playerInput.Player.CameraRotationY.ReadValue<float>() * MouseSensitivity;
        } 

        public void SetMouseSensitivity(float mouseSensitivity)
        {
            MouseSensitivity = mouseSensitivity;
            PlayerPrefs.SetFloat(PLAYER_PREFS_MOUSE_SENSITIVITY, mouseSensitivity);
            PlayerPrefs.Save();
        }


        public void RebindBinding(string inputActionId, int bindingIndex, Action afterBindAction, Action<string> duplicateBindAaction)
        {
            InputAction inputAction = playerInput.FindAction(inputActionId);
            playerInput.Disable();
            
            inputAction.PerformInteractiveRebinding(bindingIndex)
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.1f)
                .OnCancel(callback =>
                {
                    callback.Dispose();
                    playerInput.Enable();
                    afterBindAction();
                })
                .OnComplete(callback =>
                {
                    playerInput.Enable();

                    if (CheckDuplicateBindings(inputAction, bindingIndex))
                    {
                        inputAction.RemoveBindingOverride(bindingIndex);
                        callback.Dispose();
                        duplicateBindAaction("KEY IS USED");
                        RebindBinding(inputActionId, bindingIndex, afterBindAction, duplicateBindAaction);
                        return;
                    }

                    callback.Dispose();
                    afterBindAction();

                    PlayerPrefs.SetString(PLAYER_PREFS_BINDING, playerInput.SaveBindingOverridesAsJson());
                    PlayerPrefs.Save();

                })
               .Start();
        }

       
        private bool CheckDuplicateBindings(InputAction inputAction, int bindingIndex)
        {
            InputBinding newInputBinding = inputAction.bindings[bindingIndex];
            foreach (InputBinding inputBinding in playerInput.bindings)
            {
                if (inputBinding.id == newInputBinding.id)
                    continue;

                if (inputBinding.effectivePath == newInputBinding.effectivePath)
                    return true;
            }

            return false;
        }

        public string GetBindingText(string inputActionId, int bindingIndex)
        {
            return playerInput.FindAction(inputActionId).bindings[bindingIndex].ToDisplayString();
        }
    }
}
