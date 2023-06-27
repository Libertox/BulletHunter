using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float sprintBust;

        [SerializeField] private float jumpHeight;
        [SerializeField] private Transform orientationPoint;
        [SerializeField] private LayerMask groundLayerMask;

        [SerializeField] private BoxCollider squatColider;
        [SerializeField] private BoxCollider uprightColider;

        [SerializeField] private PlayerAnimation playerAnimation;


        private float jumpForce;
        private float gravityScale = 10f;
        private Rigidbody rgb;

        private float rotationX;
        private Vector3 moveDirection;
        private bool isSquat;
  
        private void Awake()
        {
            rgb = GetComponent<Rigidbody>();
            jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * gravityScale));   
        }

        private void Start()
        {
            GameInput.Instance.OnJumped += GameInput_OnJumped;
            GameInput.Instance.OnSquat += GameInput_OnSquat;
        }

        private void GameInput_OnSquat(object sender, System.EventArgs e)
        {
            Squat();
        }

        private void GameInput_OnJumped(object sender, System.EventArgs e)
        {
            HandleJump();
        }

        private void Update()
        {
            HandleRotate();
        }
        private void FixedUpdate()
        {
            HandleMovement();
 
        }

        private void HandleMovement()
        {
            //if (!GroundCheck()) return;

            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
            float speed = GameInput.Instance.GetSprintValue() * sprintBust  + movementSpeed;

            if(inputVector.y != 0)
                moveDirection =  (orientationPoint.forward.normalized) * (inputVector.y * speed * Time.deltaTime);
           else if(inputVector.x != 0)
                moveDirection = (orientationPoint.right.normalized) * (inputVector.x  * speed * Time.deltaTime);

            if(inputVector != Vector2.zero)
                rgb.velocity = new Vector3(moveDirection.x, rgb.velocity.y, moveDirection.z);
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
            if(isSquat)
            {
                uprightColider.enabled = false;
                squatColider.enabled = true;
            }
            else
            {
                uprightColider.enabled = true;
                squatColider.enabled = false;
            }

            playerAnimation.SquatAnimation(isSquat);

        }
    }

}
