using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shooter
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float sprintBust;

        [SerializeField] private float jumpHeight;
        [SerializeField] private Transform orientationPoint;
        [SerializeField] private LayerMask groundLayerMask;


        private float jumpForce;
        private float gravityScale = 10f;
        private Rigidbody rgb;

        private float rotationX;

        private void Awake()
        {
            rgb = GetComponent<Rigidbody>();
            jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics.gravity.y * gravityScale));   
        }

        private void Start()
        {
            GameInput.Instance.OnJumped += GameInput_OnJumped;
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
            if (!GroundCheck()) return;

            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
            float speed = GameInput.Instance.GetSprintValue() * sprintBust  + movementSpeed;

            if(inputVector.y != 0)
                rgb.velocity =  (orientationPoint.forward.normalized) * (inputVector.y * speed * Time.deltaTime);
           else if(inputVector.x != 0)
                rgb.velocity = (orientationPoint.right.normalized) * (inputVector.x  * speed * Time.deltaTime);
        }

        private void HandleJump()
        {
            Vector3 jumpDirection = new Vector3(orientationPoint.forward.x * movementSpeed * Time.deltaTime, jumpForce, orientationPoint.forward.z * movementSpeed * Time.deltaTime);

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
    }

}
