﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class GameInput: MonoBehaviour
    {
        public event EventHandler OnJumped;

        public static GameInput Instance { get; private set; }

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

        }

        private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnJumped?.Invoke(this, EventArgs.Empty);
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 movment = playerInput.Player.Move.ReadValue<Vector2>();
            return movment.normalized;

        }

        public float GetMouseXAxis()
        {
            return playerInput.Player.CameraRotationX.ReadValue<float>();
        }

        public float GetMouseYAxis()
        {
            return playerInput.Player.CameraRotationY.ReadValue<float>();
        }

    }
}
