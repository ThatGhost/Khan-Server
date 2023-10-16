using System;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;

public class ColliderImporter : MonoBehaviour
{
    public string path;
    public Transform root;
    public PhysicMaterial defaultMaterial;
    public string assetPath;

    private void Start()
    {
        string json = File.ReadAllText(path);

        AllColliders colliders = (AllColliders)JsonUtility.FromJson(json, typeof(AllColliders));

        foreach (var boxCollider in colliders.boxColliders)
        {
            GameObject newGameObject = new GameObject("BoxCollider");
            newGameObject.transform.position = new Vector3(boxCollider.x, boxCollider.y, boxCollider.z);
            newGameObject.transform.localScale = new Vector3(boxCollider.scalex, boxCollider.scaley, boxCollider.scalez);
            newGameObject.transform.rotation = Quaternion.Euler(boxCollider.rotx, boxCollider.roty, boxCollider.rotz);

            UnityEngine.BoxCollider box = newGameObject.AddComponent<UnityEngine.BoxCollider>();
            box.size = new Vector3(boxCollider.sizex, boxCollider.sizey, boxCollider.sizez);
            box.center = new Vector3(boxCollider.centerX, boxCollider.centerY, boxCollider.centerZ);
            box.sharedMaterial = defaultMaterial;
            box.material = defaultMaterial;

            newGameObject.transform.SetParent(root);
        }

        foreach (var capsuleColliser in colliders.capsuleColliders)
        {
            GameObject newGameObject = new GameObject("CapsuleCollider");
            newGameObject.transform.position = new Vector3(capsuleColliser.x, capsuleColliser.y, capsuleColliser.z);
            newGameObject.transform.rotation = Quaternion.Euler(capsuleColliser.rotx, capsuleColliser.roty, capsuleColliser.rotz);
            newGameObject.transform.localScale = new Vector3(capsuleColliser.scalex, capsuleColliser.scaley, capsuleColliser.scalez);

            UnityEngine.CapsuleCollider box = newGameObject.AddComponent<UnityEngine.CapsuleCollider>();
            box.center = new Vector3(capsuleColliser.centerX, capsuleColliser.centerY, capsuleColliser.centerZ);
            box.height = capsuleColliser.height;
            box.radius = capsuleColliser.radius;
            box.direction = capsuleColliser.direction;
            box.sharedMaterial = defaultMaterial;
            box.material = defaultMaterial;

            newGameObject.transform.SetParent(root);
        }

        foreach (var sphereCollider in colliders.SphereColliders)
        {
            GameObject newGameObject = new GameObject("SphereCollider");
            newGameObject.transform.position = new Vector3(sphereCollider.x, sphereCollider.y, sphereCollider.z);
            newGameObject.transform.rotation = Quaternion.Euler(sphereCollider.rotx, sphereCollider.roty, sphereCollider.rotz);
            newGameObject.transform.localScale = new Vector3(sphereCollider.scalex, sphereCollider.scaley, sphereCollider.scalez);

            UnityEngine.SphereCollider box = newGameObject.AddComponent<UnityEngine.SphereCollider>();
            box.radius = sphereCollider.radius;
            box.center = new Vector3(sphereCollider.centerX, sphereCollider.centerY, sphereCollider.centerZ);
            box.sharedMaterial = defaultMaterial;
            box.material = defaultMaterial;

            newGameObject.transform.SetParent(root);
        }

        string[] aliveAssets = AssetDatabase.FindAssets("", new[] { assetPath });
        foreach (string guid1 in aliveAssets)
        {
            Debug.Log(AssetDatabase.GUIDToAssetPath(guid1));
        }

        int number = 0;
        foreach (var meshCollider in colliders.meshColliders)
        {
            number++;
            GameObject newGameObject = new GameObject("MeshCollider");
            newGameObject.transform.position = new Vector3(meshCollider.x, meshCollider.y, meshCollider.z);
            newGameObject.transform.rotation = Quaternion.Euler(meshCollider.rotx, meshCollider.roty, meshCollider.rotz);
            newGameObject.transform.localScale = new Vector3(meshCollider.scalex, meshCollider.scaley, meshCollider.scalez);

            Mesh mesh = new Mesh();
            mesh.name = "new mesh from json";
            mesh.vertices = meshCollider.vertices;
            mesh.triangles = meshCollider.triangles;
            mesh.normals = meshCollider.normals;
            AssetDatabase.CreateAsset(mesh, assetPath + "GeneratedMesh" + number + ".asset");

            UnityEngine.MeshFilter meshFilter = newGameObject.AddComponent<UnityEngine.MeshFilter>();
            meshFilter.sharedMesh = mesh;

            UnityEngine.MeshCollider box = newGameObject.AddComponent<UnityEngine.MeshCollider>();
            box.sharedMesh = mesh;
            box.convex = meshCollider.isConvex;
            box.sharedMaterial = defaultMaterial;

            newGameObject.transform.SetParent(root);
        }
    }


    [Serializable]
    private class AllColliders
    {
        public List<BoxCollider> boxColliders = new List<BoxCollider>();
        public List<CapsuleCollider> capsuleColliders = new List<CapsuleCollider>();
        public List<SphereCollider> SphereColliders = new List<SphereCollider>();
        public List<MeshCollider> meshColliders = new List<MeshCollider>();
    }

    [Serializable]
    private class BoxCollider
    {
        public float x;
        public float y;
        public float z;
        public float rotx;
        public float roty;
        public float rotz;
        public float scalex;
        public float scaley;
        public float scalez;

        public float sizex;
        public float sizey;
        public float sizez;
        public float centerX;
        public float centerY;
        public float centerZ;
    }

    [Serializable]
    private class CapsuleCollider
    {
        public float x;
        public float y;
        public float z;
        public float rotx;
        public float roty;
        public float rotz;
        public float scalex;
        public float scaley;
        public float scalez;

        public float centerX;
        public float centerY;
        public float centerZ;
        public float radius;
        public float height;
        public int direction;
    }

    [Serializable]
    private class SphereCollider
    {
        public float x;
        public float y;
        public float z;
        public float rotx;
        public float roty;
        public float rotz;
        public float scalex;
        public float scaley;
        public float scalez;

        public float radius;
        public float centerX;
        public float centerY;
        public float centerZ;
    }

    [Serializable]
    private class MeshCollider
    {
        public float x;
        public float y;
        public float z;
        public float rotx;
        public float roty;
        public float rotz;
        public float scalex;
        public float scaley;
        public float scalez;
        public bool isConvex;
        public int[] triangles;
        public Vector3[] vertices;
        public Vector3[] normals;
    }
}
