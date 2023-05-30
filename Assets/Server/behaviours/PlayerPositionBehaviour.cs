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
        public float friction = 1f;

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

        private bool isGrounded = true;
        private Vector3 m_velocity;

        public Vector2 FaceRotation => new Vector2(face.transform.rotation.eulerAngles.x, face.transform.rotation.eulerAngles.y);
        public Transform Face
        {
            get => face;
        }

        private void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_targetRotation = transform.rotation;
            m_cameraTargetRotation = Camera.main.transform.localRotation;
        }

        public void updateInput(SInput input)
        {
            moveLeft = (input.keys & 1) > 0;
            moveRight = (input.keys & 2) > 0;
            moveUp = (input.keys & 4) > 0;
            moveDown = (input.keys & 8) > 0;
            jump = (input.keys & 16) > 0;
            moveX = input.x;
            moveY = input.y;
        }

        private void FixedUpdate()
        {
            // Rotation
            face.transform.localRotation = Quaternion.Euler(moveX, moveY, 0f);

            if (IsGrounded())
            {
                if (!isGrounded) m_velocity.y = 0;
                isGrounded = true;
                if (jump) AddForce(Vector3.up * jumpForce);
            }
            else
            {
                isGrounded = false;
                AddForce(Vector3.down * gravity);
            }

            // Movement
            Vector3 moveDirection = CalculateMoveDirection();
            AddForce(moveDirection * moveSpeed);

            ApplyForce();
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

        private void ApplyForce()
        {
            m_rigidbody.velocity = m_velocity;
            m_velocity = Vector3.Lerp(m_velocity, Vector3.zero, Time.fixedDeltaTime * friction);
        }

        private void AddForce(Vector3 force)
        {
            m_velocity += force;
            float yvel = m_velocity.y;
            m_velocity = Vector3.ClampMagnitude(m_velocity, maxVelocity);
            m_velocity.y = yvel;
        }
    }
}
