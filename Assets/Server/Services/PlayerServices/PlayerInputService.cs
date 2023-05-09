using System;
using Zenject;
using Networking.Behaviours;
using System.Linq;
using Khan_Shared.Simulation;

namespace Networking.Services
{
    public class PlayerInputService : IPlayerInputService
    {
        [Inject] private readonly ILoggerService logger;
        [Inject] private readonly PlayersController m_playersController;

        public void ReceivePlayerInput(SInput[] input, int connection)
        {
            Nullable<PlayerRefrenceObject> playerRefrenceObject = m_playersController.getPlayer(connection);
            if (playerRefrenceObject == null)
                throw new Exception("err.services.player.notfoud");
            else
                playerRefrenceObject.Value._playerPositionBehaviour.updateInput(input);
        }
    }
}
