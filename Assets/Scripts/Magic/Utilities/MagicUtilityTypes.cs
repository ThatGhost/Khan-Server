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
        public Vector3 getPlacementPoint(PlayerRefrenceObject playerRefrence, bool onGround);
    }

    public interface ISpellNetworkingUtillity
    {
        public void sendPreTrigger(int playerSpellId, int connectionId);
        public void sendAOETrigger(int playerSpellId, int connectionId, Vector3 position);
    }
}
