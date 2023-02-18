using UnityEngine;
using System.Collections;
using Khan_Shared.Simulation;

namespace Networking.Behaviours
{
    public class PlayerPositionBehaviour : MonoBehaviour, IPlayerPositionBehaviour
    {
        [SerializeField] private int m_speed;
        [SerializeField] private int m_jumpHeight;

        private readonly int m_granuality = 1000; //mm
        private PositionVector m_bigPosition;

        private void Start()
        {
            m_bigPosition = new PositionVector()
            {
                x = (int)(gameObject.transform.position.x * m_granuality),
                y = (int)(gameObject.transform.position.y * m_granuality),
                z = (int)(gameObject.transform.position.z * m_granuality),
            };
        }

        private void Update()
        {
            gameObject.transform.position = positionVectorToVector3(m_bigPosition);
        }

        public void updateInput(byte[] inputs)
        {
            foreach (var input in inputs)
            {
                PositionVector addedPosition = PlayerMovement.calculatePosition((byte)input, m_speed, m_jumpHeight, true);
                m_bigPosition += addedPosition;
            }
        }

        private Vector3 positionVectorToVector3(PositionVector positionVector)
        {
            return new Vector3()
            {
                x = positionVector.x / (float)m_granuality,
                y = positionVector.y / (float)m_granuality,
                z = positionVector.z / (float)m_granuality,
            };
        }
    }
}
