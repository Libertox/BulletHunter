using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerController : MonoBehaviour
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
            GameInput.Instance.OnJumped += GameInput_OnJumped;
            GameInput.Instance.OnSquat += GameInput_OnSquat;

            transform.position = GameManager.Instance.GetRandomPosition();

        }
    
        private void GameInput_OnSquat(object sender, System.EventArgs e) => HandleSquat();
       
        private void GameInput_OnJumped(object sender, System.EventArgs e) => HandleJump();

        private void Update() 
        {
            if (!playerInteract.GroundCheck() && rgb.velocity.y < 0)
            {
                OnFalled?.Invoke(this, trueState);
                OnJumped?.Invoke(this, falseState);
            }
            else
            {
                OnFalled?.Invoke(this, falseState);
            }


            if (Input.GetKeyDown(KeyCode.M))
                transform.position = GameManager.Instance.GetRandomPosition();
        }
      
        private void FixedUpdate() => HandleMovement();

        private void HandleMovement()
        {
            if (isClimb || !playerInteract.GroundCheck()) return;
       
            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

            if (GameInput.Instance.GetSprintValue() == 0 || inputVector == Vector2.zero)
            {
                PlayerStats.IncreaseStamina(Time.deltaTime);
                InvokeSprintEvent(false);
            }

            if (inputVector == Vector2.zero)
            {
                OnWalked?.Invoke(this, falseState);
                return;
            }

            OnWalked?.Invoke(this, trueState) ;

            if(GameInput.Instance.GetSprintValue() == 1)
                PlayerStats.DecreaseStamina(Time.deltaTime);

            float movementSpeed = GetMovementSpeed();

            moveDirection = (orientationPoint.forward.normalized * inputVector.y + orientationPoint.right.normalized * inputVector.x) * movementSpeed;

            rgb.velocity = new Vector3(moveDirection.x, rgb.velocity.y, moveDirection.z);
        }

        private float GetMovementSpeed()
        {
            if (PlayerStats.Stamina > 0)
            {
                if(GameInput.Instance.GetSprintValue() == 1)
                    InvokeSprintEvent(true);

                return GameInput.Instance.GetSprintValue() * sprintBust + baseSpeed;
            }
            else
            {
                InvokeSprintEvent(false);
                return baseSpeed;
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

            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
            moveDirection = (orientationPoint.forward.normalized * inputVector.y + orientationPoint.right.normalized * inputVector.x) * baseSpeed;
            Vector3 jumpDirection = new Vector3(moveDirection.x, jumpForce, moveDirection.z);

            if (playerInteract.GroundCheck())
            {
                rgb.AddForce(jumpDirection, ForceMode.Impulse);
                OnJumped?.Invoke(this, trueState);
            }
                         
        }

        public void HandleRotate(Vector3 angle) => transform.eulerAngles = angle;
     
        private void HandleSquat()
        {
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

            rgb.velocity = new Vector3(moveDirection.x, inputVector.y, 0);
            if (!playerInteract.GroundCheck() || inputVector.y >= 0)
                isClimb = true;
            else
                isClimb = false;
        }

        public void DropLadder()
        {
            rgb.useGravity = true;
            isClimb = false;

            if (!playerInteract.GroundCheck())
            {
                rgb.velocity = (Vector3.up  + orientationPoint.forward) * 2f;
            }
       
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
