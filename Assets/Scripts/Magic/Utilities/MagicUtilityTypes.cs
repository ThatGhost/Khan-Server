using System.Collections;
using System.Collections.Generic;
using Khan_Shared.Magic;
using Networking.Services;
using UnityEngine;

namespace Server.Magic
{
    public interface ISpellClickUtillity
    {
        public void clicked(bool clicked);
        public bool isDown(bool clicked);
        public bool isUp(bool clicked);
        public bool isHeld(bool clicked);
        public bool isAny(bool clicked);
    }

    public interface ISpellPoolUtillity
    {
        public T request<T>(Spell spell) where T : PrefabBuilder;
        public void release(GameObject gameObject);
        public void setup(GameObject prefab);
        public void destruct();
    }

    public interface ISpellPlayerUtillity
    {
        public Vector3 getGroundPoint(PlayerRefrenceObject playerRefrence);
        public Vector3 getLookPoint(PlayerRefrenceObject playerRefrence);
        public Vector3 getLookDirection(PlayerRefrenceObject playerRefrence, bool euler);
    }

    public interface ISpellNetworkingUtillity
    {
        public void sendPostTrigger(int playerSpellId, int connectionId, bool wasValid);
        public void sendAOETrigger(int playerSpellId, int connectionId, Vector3 position);
        public void sendPlacementTrigger(int playerSpellId, int connectionId, Vector3 position, Vector3 rotation);
    }
}
