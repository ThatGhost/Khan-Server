using System;
using Khan_Shared.Simulation;
using UnityEngine;

namespace Networking.Behaviours
{
    public interface ITestBehaviour
    {
        public void doSomething();
    }

    public interface IPlayerPositionBehaviour
    {
        public void updateInput(SInput[] inputs);
        public Vector2 FaceRotation { get; }
    }
}
