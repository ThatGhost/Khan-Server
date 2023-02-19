using System;
using Zenject;
using Networking.Behaviours;
using System.Linq;
using Khan_Shared.Simulation;

namespace Networking.Services
{
    public class PlayerInputService : IPlayerInputService
    {
        [Inject] private readonly IPlayerPositionBehaviour m_playerPositionBehaviour;
        [Inject] private readonly ILogger logger;

        public void ReceivePlayerInput(SInput[] input, int connection)
        {
            m_playerPositionBehaviour.updateInput(input);
        }
    }
}
