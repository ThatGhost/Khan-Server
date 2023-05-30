using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ColliderImporter : MonoBehaviour
{
    public string path;
    public Transform root;

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

            newGameObject.transform.SetParent(root);
        }

        foreach (var sphereCollider in colliders.SphereColliders)
        {
            GameObject newGameObject = new GameObject("CapsuleCollider");
            newGameObject.transform.position = new Vector3(sphereCollider.x, sphereCollider.y, sphereCollider.z);
            newGameObject.transform.rotation = Quaternion.Euler(sphereCollider.rotx, sphereCollider.roty, sphereCollider.rotz);
            newGameObject.transform.localScale = new Vector3(sphereCollider.scalex, sphereCollider.scaley, sphereCollider.scalez);

            UnityEngine.SphereCollider box = newGameObject.AddComponent<UnityEngine.SphereCollider>();
            box.radius = sphereCollider.radius;
            box.center = new Vector3(sphereCollider.centerX, sphereCollider.centerY, sphereCollider.centerZ);

            newGameObject.transform.SetParent(root);
        }
    }


    [Serializable]
    private class AllColliders
    {
        public List<BoxCollider> boxColliders = new List<BoxCollider>();
        public List<CapsuleCollider> capsuleColliders = new List<CapsuleCollider>();
        public List<SphereCollider> SphereColliders = new List<SphereCollider>();
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
}
