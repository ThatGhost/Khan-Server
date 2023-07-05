﻿using System;
using Khan_Shared.Utils;
using UnityEngine;

namespace Networking.Behaviours
{
    public interface ITestBehaviour
    {
        public void doSomething();
    }

    public interface IPlayerPositionBehaviour
    {
        public void updateInput(SInput input);
        public void AddForce(Vector3 force);
        public Vector2 FaceRotation { get; }
    }

    public interface ISpellUtil_BasicTimer
    {
        event Action onStartUp;
        event Action onFireStart;
        event Action onFireEnd;
        event Action onEnd;

        public void start(float startUpTime, float fireTime, float endTime);
    }

    public interface IDebugStatBehaviour
    {
        public void addBytesOut(int bytes);
        public void addMessagesOut(int messages);
    }
}
