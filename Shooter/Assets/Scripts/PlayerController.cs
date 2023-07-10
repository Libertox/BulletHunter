using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerController : MonoBehaviour
    {
        public static event EventHandler<OnSquatedEventArgs> OnSquated;
        public static event EventHandler<OnSprintedEventArgs> OnSprinted;

        public class OnSquatedEventArgs : EventArgs { public bool isSquat; }
        public class OnSprintedEventArgs: EventArgs { public bool isSprint, isSquat; }

        public PlayerStats PlayerStats { get; private set; }

        [SerializeField] private float movementSpeed;
        [SerializeField] private float sprintBust;
        [SerializeField] private float jumpHeight;
        [SerializeField] private Transform orientationPoint;

        private readonly float gravityScale = 10f;

        private Rigidbody rgb;
        private PlayerInteract playerInteract;

        private float jumpForce;
        private float rotationX;

        private Vector3 moveDirection;
        private bool isSquat;

      
        private void Awake()
        {
            rgb = GetComponent<Rigidbody>();
            PlayerStats = GetComponent<PlayerStats>();
            playerInteract = GetComponent<PlayerInteract>();
            jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * gravityScale));   
        }

        private void Start()
        {
            GameInput.Instance.OnJumped += GameInput_OnJumped;
            GameInput.Instance.OnSquat += GameInput_OnSquat;

        }
    
        private void GameInput_OnSquat(object sender, System.EventArgs e) => HandleSquat();
       
        private void GameInput_OnJumped(object sender, System.EventArgs e) => HandleJump();
      
        private void Update() => HandleRotate();
      
        private void FixedUpdate() => HandleMovement();

        private void HandleMovement()
        {
            if (!playerInteract.GroundCheck()) return;

            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

            if (GameInput.Instance.GetSprintValue() == 0 || inputVector == Vector2.zero)
            {
                PlayerStats.IncreaseStamina(Time.deltaTime);
                InvokeSprintEvent(false);
            }
                
            if (inputVector == Vector2.zero) return;  

            if(GameInput.Instance.GetSprintValue() == 1)
                PlayerStats.DecreaseStamina(Time.deltaTime);

            float speed = GetMoveSpeed();

            moveDirection = (orientationPoint.forward.normalized * inputVector.y + orientationPoint.right.normalized * inputVector.x) * speed;

            rgb.velocity = new Vector3(moveDirection.x, rgb.velocity.y, moveDirection.z);
        }

        private float GetMoveSpeed()
        {
            if (PlayerStats.Stamina > 0)
            {
                if(GameInput.Instance.GetSprintValue() == 1)
                    InvokeSprintEvent(true);

                return GameInput.Instance.GetSprintValue() * sprintBust + movementSpeed;
            }
            else
            {
                InvokeSprintEvent(false);
                return movementSpeed;
            }
                
        }

        private void InvokeSprintEvent(bool isSprint)
        {
            OnSprinted?.Invoke(this, new OnSprintedEventArgs
            {
                isSprint = isSprint,
                isSquat = isSquat
            });
        }

        private void HandleJump()
        {
            if (isSquat) return;

            Vector3 jumpDirection = new Vector3(rgb.velocity.x, jumpForce, rgb.velocity.z);

            if (playerInteract.GroundCheck())
                rgb.AddForce(jumpDirection, ForceMode.Impulse);            
        }

        private void HandleRotate()
        {
            rotationX += GameInput.Instance.GetMouseXAxis();
            transform.eulerAngles = new Vector3(transform.rotation.x, rotationX, transform.rotation.z);
        }

        private void HandleSquat()
        {
            isSquat = !isSquat;
            OnSquated?.Invoke(this, new OnSquatedEventArgs
            {
                isSquat = this.isSquat
            });

        }
    }

}
