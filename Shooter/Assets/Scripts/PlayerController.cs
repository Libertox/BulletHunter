using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shooter
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private Transform orientationPoint;
        private float jumpForce;
        private float gravityScale = 10f;
        private Rigidbody rgb;

        private float rotationX;
        private float rotationY;

        private void Awake()
        {
            rgb = GetComponent<Rigidbody>();
            jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * gravityScale));   
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
            HandleMovement();
            HandleRotate();
        }

        private void HandleMovement()
        {
            Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

            if(inputVector.y != 0)
                rgb.velocity =  orientationPoint.forward.normalized * inputVector.y  * (movementSpeed * Time.deltaTime);
           else if(inputVector.x != 0)
                rgb.velocity = orientationPoint.right.normalized * inputVector.x * (movementSpeed * Time.deltaTime);
        }

        private void HandleJump()
        {
            rgb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
        
        private void HandleRotate()
        {
            rotationX += GameInput.Instance.GetMouseXAxis();
            rotationX = Mathf.Clamp(rotationX, -80, 80);
            transform.eulerAngles = new Vector3(transform.rotation.x, rotationX, transform.rotation.z);
        }

        private Vector3 MultiplicationVector(Vector3 vector, Vector3 vector1)
        {
            return new Vector3(vector.x * vector1.x, vector.y * vector1.y, vector.z * vector1.z);
        }


    }

}
