using System;
using Khan_Shared.Simulation;

namespace Networking.Behaviours
{
    public interface ITestBehaviour
    {
        public void doSomething();
    }

    public interface IPlayerPositionBehaviour
    {
        public void updateInput(SInput[] inputs);
    }
}
