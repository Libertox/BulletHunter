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

        [Header("Basic Attributess")]
        [SerializeField] private float movementSpeed;
        [SerializeField] private float sprintBust;
        [SerializeField] private float jumpHeight;
        [SerializeField] private Transform orientationPoint;

     
        [Header("Colid  Attributess")]
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private BoxCollider squatColider;
        [SerializeField] private BoxCollider uprightColider;

        [Space(10)]
       
        [SerializeField] private PlayerAnimation playerAnimation;
     
        private readonly float gravityScale = 10f;

        private Rigidbody rgb;
        private float jumpForce;
        private PlayerStats playerStats;
        private float rotationX;
        private Vector3 moveDirection;
        private bool isSquat;

      
        private void Awake()
        {
            rgb = GetComponent<Rigidbody>();
            playerStats = GetComponent<PlayerStats>();
            jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * gravityScale));   
        }

        private void Start()
        {
            GameInput.Instance.OnJumped += GameInput_OnJumped;
            GameInput.Instance.OnSquat += GameInput_OnSquat;

        }

    
        private void GameInput_OnSquat(object sender, System.EventArgs e) => Squat();
       
        private void GameInput_OnJumped(object sender, System.EventArgs e) => HandleJump();
      
        private void Update() => HandleRotate();
      
        private void FixedUpdate() => HandleMovement();
       
        private void HandleMovement()
        {
            //if (!GroundCheck()) return;
            
            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

            if (inputVector == Vector2.zero) return;

            float speed = GetMoveSpeed();

            if (inputVector.y != 0)
                moveDirection =  (orientationPoint.forward.normalized) * (inputVector.y * speed * Time.deltaTime);
           else if(inputVector.x != 0)
                moveDirection = (orientationPoint.right.normalized) * (inputVector.x  * speed * Time.deltaTime);

             rgb.velocity = new Vector3(moveDirection.x, rgb.velocity.y, moveDirection.z);
        }
        private float GetMoveSpeed()
        {
            if (playerStats.Stamina > 0)
            {
                if (GameInput.Instance.GetSprintValue() == 0)
                    InvokeSprintEvent(false);
                else
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

            if (GroundCheck())
                rgb.AddForce(jumpDirection, ForceMode.Impulse);            
        }

        private bool GroundCheck()
        {
            Vector3 boxSize = new Vector3(0.6f, 0.6f, 0.6f);
            if (Physics.CheckBox(transform.position, boxSize, Quaternion.identity, groundLayerMask))
                return true;

            return false;
        }
        
        private void HandleRotate()
        {
            rotationX += GameInput.Instance.GetMouseXAxis();
            transform.eulerAngles = new Vector3(transform.rotation.x, rotationX, transform.rotation.z);
        }

        private void Squat()
        {
            isSquat = !isSquat;
            OnSquated?.Invoke(this, new OnSquatedEventArgs
            {
                isSquat = this.isSquat
            });

        }
    }

}
