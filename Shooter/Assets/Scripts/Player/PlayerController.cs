using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace BulletHaunter
{
    public class PlayerController : NetworkBehaviour
    {
        public static event EventHandler<OnStateChangedEventArgs> OnSquated;
        public static event EventHandler<OnSprintedEventArgs> OnSprinted;
        public static event EventHandler<OnStateChangedEventArgs> OnWalked;
        public static event EventHandler<OnStateChangedEventArgs> OnJumped;
        public static event EventHandler<OnStateChangedEventArgs> OnFalled;

        public class OnSprintedEventArgs: EventArgs { public bool isSprint, isSquat; }

        public class OnStateChangedEventArgs : EventArgs { public bool state;};

        public PlayerStats PlayerStats { get; private set; }

        [SerializeField] private float baseSpeed;
        [SerializeField] private float sprintBust;
        [SerializeField] private float jumpHeight;
        [SerializeField] private Transform orientationPoint;

        private readonly float gravityScale = 10f;

        private OnStateChangedEventArgs trueState;
        private OnStateChangedEventArgs falseState;

        private Rigidbody rgb;
        private PlayerInteract playerInteract;

        private float jumpForce;
        private Vector3 moveDirection;

        private bool isSquat;
        private bool isClimb;
        private bool isSprint;
        private bool isGround;

        private void Awake()
        {
            rgb = GetComponent<Rigidbody>();
            PlayerStats = GetComponent<PlayerStats>();
            playerInteract = GetComponent<PlayerInteract>();
            jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * gravityScale));

            trueState = new OnStateChangedEventArgs { state = true };
            falseState = new OnStateChangedEventArgs { state = false };
        }

        private void Start()
        {
            if (!IsOwner) return;

            GameInput.Instance.OnJumped += GameInput_OnJumped;
            GameInput.Instance.OnSquat += GameInput_OnSquat;

            PlayerStats.OnDeathed += PlayerStats_OnDeathed;
            PlayerStats.OnRestored += PlayerStats_OnRestored;
        }

        private void PlayerStats_OnRestored(object sender, EventArgs e)
        {
            rgb.useGravity = true;
            isSquat = false;
            OnSquated?.Invoke(this, new OnStateChangedEventArgs
            {
                state = isSquat
            });

        }

        private void PlayerStats_OnDeathed(object sender, PlayerStats.OnDeathedEventArgs e) => rgb.useGravity = false;
      
        private void GameInput_OnSquat(object sender, System.EventArgs e) => HandleSquat();
       
        private void GameInput_OnJumped(object sender, System.EventArgs e) => HandleJump();

        private void Update() 
        {
            if (!IsOwner) return;

            isSprint = !isSquat && GameInput.Instance.GetSprintValue() == 1;
            isGround = playerInteract.GroundCheck();

            if (!isGround && rgb.velocity.y < 0)
            {
                OnFalled?.Invoke(this, trueState);
                OnJumped?.Invoke(this, falseState);
            }
            else
                OnFalled?.Invoke(this, falseState);
        }

        private void FixedUpdate() 
        {
            if (!IsOwner) return;

            HandleMovement();
        } 

        private void HandleMovement()
        {
            if (isClimb || !playerInteract.GroundCheck()) return;
       
            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

            if (!isSprint || inputVector == Vector2.zero)
            {
                PlayerStats.IncreaseStamina(Time.deltaTime);
                OnSprinted?.Invoke(this, new OnSprintedEventArgs
                {
                    isSprint = isSprint,
                    isSquat = isSquat
                });
            }

            if (inputVector == Vector2.zero)
            {
                OnWalked?.Invoke(this, falseState);
                return;
            }

            OnWalked?.Invoke(this, trueState);

            if(isSprint)
                PlayerStats.DecreaseStamina(Time.deltaTime);

            float movementSpeed = GetMovementSpeed();

            moveDirection = (orientationPoint.forward.normalized * inputVector.y + orientationPoint.right.normalized * inputVector.x) * movementSpeed;

            rgb.velocity = new Vector3(moveDirection.x, rgb.velocity.y, moveDirection.z);
        }

        private float GetMovementSpeed()
        {
            if (PlayerStats.Stamina > 0 && isSprint)
            {
                OnSprinted?.Invoke(this, new OnSprintedEventArgs
                {
                    isSprint = isSprint,
                    isSquat = isSquat
                });

                return GameInput.Instance.GetSprintValue() * sprintBust + baseSpeed;
            }


            OnSprinted?.Invoke(this, new OnSprintedEventArgs
            {
                isSprint = isSprint,
                isSquat = isSquat
            });
            return baseSpeed;      
        }

        private void HandleJump()
        {
            if (isSquat) return;

            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
            moveDirection = (orientationPoint.forward.normalized * inputVector.y + orientationPoint.right.normalized * inputVector.x) * baseSpeed;
            Vector3 jumpDirection = new Vector3(moveDirection.x, jumpForce, moveDirection.z);

            if (isGround)
            {
                rgb.AddForce(jumpDirection, ForceMode.Impulse);
                OnJumped?.Invoke(this, trueState);
            }
                         
        }

        public void HandleRotate(Vector3 angle) => transform.eulerAngles = angle;
     
        private void HandleSquat()
        {
            if (isClimb) return;

            isSquat = !isSquat;
            OnSquated?.Invoke(this, new OnStateChangedEventArgs
            {
                state = isSquat
            });

        }

        public void ClimbOnLadder()
        {
            rgb.useGravity = false;
            
            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

            moveDirection = (orientationPoint.right.normalized * inputVector.x) * baseSpeed;

            rgb.velocity = new Vector3(moveDirection.x, inputVector.y, moveDirection.z);
            if (!isGround || inputVector.y >= 0)
                isClimb = true;
            else
                isClimb = false;
        }

        public void GetOffLader()
        {
            rgb.useGravity = true;
            isClimb = false;

            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
            float getOffLaderSpeed = 2f;

            if (!isGround)
                rgb.velocity = getOffLaderSpeed * inputVector.y * (Vector3.up  + orientationPoint.forward);   
        }

        public static void ResetStaticData()
        {
            OnSquated = null;
            OnSprinted = null;
            OnWalked = null;
            OnJumped = null;
            OnFalled = null;
        }


    }

}
