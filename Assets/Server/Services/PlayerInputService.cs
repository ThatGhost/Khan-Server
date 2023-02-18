using System;
using Zenject;
using Networking.Behaviours;
using System.Linq;

namespace Networking.Services
{
    public class PlayerInputService : IPlayerInputService
    {
        [Inject] private readonly IPlayerPositionBehaviour m_playerPositionBehaviour;

        public void ReceivePlayerInput(byte[] input, int connection)
        {
            m_playerPositionBehaviour.updateInput(input.Skip(2).ToArray());
        }
    }
}
