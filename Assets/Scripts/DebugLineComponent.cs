using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLineComponent : MonoBehaviour
{
    [SerializeField] private float m_lenght;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * m_lenght);
    }
}
