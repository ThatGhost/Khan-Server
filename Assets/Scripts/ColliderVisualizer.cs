using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using static UnityEngine.GraphicsBuffer;

public class ColliderVisualizer : MonoBehaviour
{
    public bool showColliders = true;

    private void OnDrawGizmos()
    {
        if (!showColliders)
            return;

        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (!Selection.Contains(collider.gameObject))
            {
                DrawColliderGizmo(collider);
            }
        }
    }

    private void DrawColliderGizmo(Collider collider)
    {
        if (collider == null)
            return;

        Gizmos.color = Color.green;

        if (collider is MeshCollider)
        {
            Gizmos.color = Color.green;
            MeshCollider meshCollider = collider as MeshCollider;
            // Draw mesh-specific gizmo representation
            Gizmos.DrawWireMesh(meshCollider.sharedMesh, meshCollider.transform.position, meshCollider.transform.rotation, meshCollider.transform.localScale);
        }
        else if (collider is SphereCollider)
        {
            Gizmos.color = Color.blue;
            SphereCollider sphereCollider = collider as SphereCollider;
            Gizmos.DrawWireSphere(sphereCollider.transform.TransformPoint(sphereCollider.center), sphereCollider.radius);
        }
        else if (collider is BoxCollider)
        {
            Gizmos.color = Color.red;
            BoxCollider boxCollider = collider as BoxCollider;
            Gizmos.matrix = Matrix4x4.TRS(boxCollider.transform.position, boxCollider.transform.rotation, boxCollider.transform.lossyScale);
            Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        }
        else if (collider is CapsuleCollider)
        {
            Gizmos.color = Color.yellow;
            CapsuleCollider capsuleCollider = collider as CapsuleCollider;
            // Draw capsule-specific gizmo representation
            Gizmos.DrawWireSphere(capsuleCollider.transform.TransformPoint(capsuleCollider.center), capsuleCollider.bounds.size.z);
        }
        // Add more collider types as needed
    }
}