using UnityEngine;
using System.Collections;
using Khan_Shared.Utils;
using UnityEngine.Windows;
using Zenject;

namespace Networking.Behaviours
{
    public class PlayerPositionBehaviour : MonoBehaviour, IPlayerPositionBehaviour
    {
        [SerializeField] private Transform face;
        [SerializeField] private Transform feet;

        public float moveSpeed = 5f;
        public float jumpForce = 5f;
        public float gravity = 9.81f;
        public float maxVelocity = 100f;

        private Rigidbody m_rigidbody;
        private Quaternion m_targetRotation;
        private Quaternion m_cameraTargetRotation;

        private bool moveUp;
        private bool moveDown;
        private bool moveLeft;
        private bool moveRight;
        private bool jump;
        private float moveX;
        private float moveY;

        public Vector2 FaceRotation => new Vector2(face.transform.rotation.eulerAngles.x, face.transform.rotation.eulerAngles.y);
        public Transform Face
        {
            get => face;
        }

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            m_targetRotation = transform.rotation;
            m_cameraTargetRotation = Camera.main.transform.localRotation;
        }

        public void updateInput(SInput[] inputs)
        {
            // Reset movement and rotation variables
            moveUp = false;
            moveDown = false;
            moveLeft = false;
            moveRight = false;
            jump = false;
            moveX = 0;
            moveY = 0;

            // Process each input in the array
            for (int i = 0; i < inputs.Length; i++)
            {
                SInput input = inputs[i];
                moveLeft |= (input.keys & 1) > 0;
                moveRight |= (input.keys & 2) > 0;
                moveUp |= (input.keys & 4) > 0;
                moveDown |= (input.keys & 8) > 0;
                jump |= (input.keys & 16) > 0;
                moveX = input.x;
                moveY = input.y;
            }
        }

        private void FixedUpdate()
        {
            // Rotation
            face.transform.localRotation = Quaternion.Euler(moveX, moveY, 0f);

            // Jump
            if (jump && IsGrounded())
            {
                m_rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            // Movement
            Vector3 moveDirection = CalculateMoveDirection();
            m_rigidbody.AddForce(moveDirection * moveSpeed);

            // Clamp velocity
            m_rigidbody.velocity = Vector3.ClampMagnitude(m_rigidbody.velocity, maxVelocity);

            // Gravity
            m_rigidbody.AddForce(Vector3.down * gravity);
        }

        private Vector3 CalculateMoveDirection()
        {
            Vector3 moveDirection = Vector3.zero;

            if (moveUp && !moveDown)
                moveDirection += face.forward;
            else if (moveDown && !moveUp)
                moveDirection -= face.forward;

            if (moveLeft && !moveRight)
                moveDirection -= face.right;
            else if (moveRight && !moveLeft)
                moveDirection += face.right;

            moveDirection.Normalize();
            moveDirection.y = 0;
            return moveDirection;
        }

        private bool IsGrounded()
        {
            float groundCheckDistance = 0.2f;
            return Physics.Raycast(feet.position, Vector3.down, groundCheckDistance);
        }
    }
}
