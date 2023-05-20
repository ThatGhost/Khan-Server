﻿using UnityEngine;
using System.Collections;
using Khan_Shared.Simulation;
using UnityEngine.Windows;

namespace Networking.Behaviours
{
    public class PlayerPositionBehaviour : MonoBehaviour, IPlayerPositionBehaviour
    {
        [SerializeField] private int m_speed;
        [SerializeField] private int m_jumpHeight;
        [SerializeField] private Transform m_face;

        private PositionVector m_bigPosition;
        private short m_bigRotX;
        private short m_bigRotY;

        private int m_realSpeed;

        public Vector2 FaceRotation => new Vector2(m_bigRotX / SimulationConfiguration.g_MouseGranulairity, m_bigRotY / SimulationConfiguration.g_MouseGranulairity);
        public Transform Face
        {
            get => m_face;
        }

        private void OnValidate()
        {
            m_realSpeed = (int)(m_speed * Time.fixedDeltaTime);
        }

        private void Start()
        {
            m_realSpeed = (int)(m_speed * Time.fixedDeltaTime);
            m_bigPosition = new PositionVector()
            {
                x = (int)(gameObject.transform.position.x * SimulationConfiguration.g_InputGranulairity),
                y = (int)(gameObject.transform.position.y * SimulationConfiguration.g_InputGranulairity),
                z = (int)(gameObject.transform.position.z * SimulationConfiguration.g_InputGranulairity),
            };
        }

        private void Update()
        {
            gameObject.transform.position = positionVectorToVector3(m_bigPosition);
            m_face.rotation = Quaternion.Euler(new Vector3()
            {
                x = (float)(m_bigRotX / SimulationConfiguration.g_MouseGranulairity),
                y = (float)(m_bigRotY / SimulationConfiguration.g_MouseGranulairity),
            });
        }

        public void receiveInput(SInput[] inputs)
        {
            PositionVector fullInputVector = new PositionVector();
            short fullx = 0;
            short fully = 0;
            foreach (var input in inputs)
            {
                fullInputVector += PlayerMovement.calculatePosition((ushort)input.keys, m_realSpeed, m_jumpHeight, true);
                fullx = input.x;
                fully = input.y;
            }
            float lenghtOfInputVector = Mathf.Sqrt((float)(fullInputVector.x * fullInputVector.x + fullInputVector.z * fullInputVector.z));
            PositionVector addedPosition = new PositionVector()
            {
                x = (int)(fullInputVector.x / lenghtOfInputVector * m_realSpeed),
                z = (int)(fullInputVector.z / lenghtOfInputVector * m_realSpeed),
            };
            m_bigPosition += rotatePositionAlongYAxis(addedPosition, fully);
            m_bigRotX = fullx;
            m_bigRotY = fully;
        }

        private Vector3 positionVectorToVector3(PositionVector positionVector)
        {
            return new Vector3()
            {
                x = positionVector.x / (float)SimulationConfiguration.g_InputGranulairity,
                y = positionVector.y / (float)SimulationConfiguration.g_InputGranulairity,
                z = positionVector.z / (float)SimulationConfiguration.g_InputGranulairity,
            };
        }

        private PositionVector rotatePositionAlongYAxis(PositionVector position, short y)
        {
            float realY = y / (float)SimulationConfiguration.g_MouseGranulairity;
            realY *= Mathf.Deg2Rad;

            float cos = Mathf.Cos(realY);
            float sin = Mathf.Sin(realY);

            PositionVector newVector = new PositionVector()
            {
                z = (int)(position.z * cos - position.x * sin),
                y = 0,
                x = (int)(position.z * sin + position.x * cos),
            };
            return newVector;
        }
    }
}
