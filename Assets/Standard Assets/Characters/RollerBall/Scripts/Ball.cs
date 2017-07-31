using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Ball
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float m_MovePower = 5; // The force added to the ball to move it.
        [SerializeField] private bool m_UseTorque = true; // Whether or not to use torque to move the ball.
        [SerializeField] private float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.
        [SerializeField] private float m_JumpPower = 2; // The force added to the ball when it jumps.

        [SerializeField] bool gravityReversed = false;
        [SerializeField] float gravityAmt, currentGravity;

        private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.
        [SerializeField] private Rigidbody m_Rigidbody;

        public bool GravityReversed
        {
            get
            {
                return gravityReversed;
            }

            set
            {
                gravityReversed = value;
            }
        }

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            // Set the maximum angular velocity.
            GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
        }

        private void OnEnable()
        {
            m_Rigidbody.useGravity = false;
        }

        private void OnDisable()
        {
            if (gravityReversed)
                SwitchGravity();
            m_Rigidbody.useGravity = true;
        }

        private void FixedUpdate()
        {
            ApplyGravity();
            
        }

        void ApplyGravity()
        {
            m_Rigidbody.AddForce(Vector3.down * currentGravity);
        }

        void SwitchGravity()
        {
            gravityReversed = !gravityReversed;
            currentGravity = gravityReversed ? -gravityAmt : gravityAmt;
        }

        public void Move(Vector3 moveDirection, bool jump)
        {
            // If using torque to rotate the ball...
            if (m_UseTorque)
            {
                // ... add torque around the axis defined by the move direction.
                m_Rigidbody.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x)*m_MovePower);
            }
            else
            {
                // Otherwise add force in the move direction.
                m_Rigidbody.AddForce(moveDirection*m_MovePower);
            }

            // If on the ground and jump is pressed...
            Vector3 checkDirection = gravityReversed ? Vector3.up : -Vector3.up;
            if (Physics.Raycast(transform.position, checkDirection, k_GroundRayLength) && jump)
            {
                SwitchGravity();
                // ... add force in upwards.
              //  m_Rigidbody.AddForce(Vector3.up*m_JumpPower, ForceMode.Impulse);
            }
        }
    }
}
