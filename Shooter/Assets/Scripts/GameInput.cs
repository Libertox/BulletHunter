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

        private PlayerInput playerInput;


        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
                Destroy(gameObject);

            playerInput = new PlayerInput();
            playerInput.Enable();

            playerInput.Player.Jump.performed += Jump_performed;
            playerInput.Player.Squat.performed += Squat_performed;

        }

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
