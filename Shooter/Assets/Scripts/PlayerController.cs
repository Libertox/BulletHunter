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
        public static event EventHandler<OnWalkedEventArgs> OnWalked;
        public static event EventHandler<OnJumpedEventArgs> OnJumped;
        public static event EventHandler<OnFalledEventArgs> OnFalled;

        public class OnSquatedEventArgs : EventArgs { public bool isSquat; }
        public class OnSprintedEventArgs: EventArgs { public bool isSprint, isSquat; }
        public class OnWalkedEventArgs: EventArgs { public bool isWalk; }
        public class OnJumpedEventArgs : EventArgs { public bool isJump; }
        public class OnFalledEventArgs: EventArgs { public bool isFall; }

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
        private bool isClimb;

      
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

        private void Update() 
        {
            HandleRotate();
            if (!playerInteract.GroundCheck() && rgb.velocity.y < 0)
            {
                OnFalled?.Invoke(this, new OnFalledEventArgs { isFall = true });
                OnJumped?.Invoke(this, new OnJumpedEventArgs { isJump = false });
            }
            else
            {
                OnFalled?.Invoke(this, new OnFalledEventArgs { isFall = false });
            }
        }
      
        private void FixedUpdate() => HandleMovement();

        private void HandleMovement()
        {
            //if (!playerInteract.GroundCheck()) return;

            if (isClimb) return;
       
            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

            if (GameInput.Instance.GetSprintValue() == 0 || inputVector == Vector2.zero)
            {
                PlayerStats.IncreaseStamina(Time.deltaTime);
                InvokeSprintEvent(false);
            }

            if (inputVector == Vector2.zero)
            {
                OnWalked?.Invoke(this, new OnWalkedEventArgs { isWalk = false });
                return;
            }

            OnWalked?.Invoke(this, new OnWalkedEventArgs { isWalk = true }) ;

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
            {
                rgb.AddForce(jumpDirection, ForceMode.Impulse);
                OnJumped?.Invoke(this, new OnJumpedEventArgs { isJump = true });
            }
                         
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

        public void ClimbOnLadder()
        {
            rgb.useGravity = false;
            isClimb = true;

            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

            moveDirection = (orientationPoint.right.normalized * inputVector.x) * 3f;

            if (!playerInteract.GroundCheck() || inputVector.y >= 0)
                rgb.velocity = new Vector3(moveDirection.x, inputVector.y, 0);
            else
                rgb.velocity = new Vector3(moveDirection.x, 0, inputVector.y);
        }

        public void DropLadder()
        {
            rgb.useGravity = true;
            isClimb = false;
        }

    }

}
