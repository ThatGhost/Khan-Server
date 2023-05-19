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
        [Inject] private readonly IPlayersController m_playersController;

        public void ReceivePlayerInput(SInput[] input, int connection)
        {
            Nullable<PlayerRefrenceObject> playerRefrenceObject = m_playersController.getPlayer(connection);
            if (playerRefrenceObject == null)
                throw new Exception("err.services.player.notfoud");
            
            playerRefrenceObject.Value._playerPositionBehaviour.receiveInput(input);
            playerRefrenceObject.Value._playerSpellController.receiveInput(input);
        }
    }
}
