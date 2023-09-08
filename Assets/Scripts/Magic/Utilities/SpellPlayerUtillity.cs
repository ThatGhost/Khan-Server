using System.Collections;
using System.Collections.Generic;
using Networking.Services;
using UnityEngine;

namespace Server.Magic
{
    public class SpellPlayerUtillity : ISpellPlayerUtillity
    {
        public Vector3 getGroundPoint(PlayerRefrenceObject playerRefrence)
        {
            Transform face = playerRefrence._playerPositionBehaviour.Face;
            int layerMask = 1 << 8; // spells can't interact with spells
            layerMask = ~layerMask;

            Vector3 position = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(face.transform.position, face.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                RaycastHit groundHit;
                if (Physics.Raycast(hit.point + hit.normal.normalized * 0.1f, Vector3.down, out groundHit, Mathf.Infinity, layerMask))
                {
                    return groundHit.point;
                }
                else return hit.point;
            }

            return Vector3.zero;
        }

        public Vector3 getLookDirection(PlayerRefrenceObject playerRefrence, bool euler)
        {
            Transform face = playerRefrence._playerPositionBehaviour.Face;
            return euler ? face.eulerAngles : face.forward;
        }

        public Vector3 getLookPoint(PlayerRefrenceObject playerRefrence)
        {
            Transform face = playerRefrence._playerPositionBehaviour.Face;
            int layerMask = 1 << 8; // spells can't interact with spells
            layerMask = ~layerMask;

            Vector3 position = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(face.transform.position, face.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                return hit.point;
            }
            return Vector3.zero;
        }
    }
}
