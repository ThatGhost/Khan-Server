using System;
using Zenject;
using Khan_Shared.Utils;

namespace Networking.Services
{
    public class PlayerInputService : IPlayerInputService
    {
        [Inject] private readonly IPlayersController m_playersController;

        public void ReceivePlayerInput(SInput input, int connection)
        {
            Nullable<PlayerRefrenceObject> playerRefrenceObject = m_playersController.getPlayer(connection);
            if (playerRefrenceObject == null)
                throw new Exception($"err.services.player.notfoud, connection {connection}");
            
            playerRefrenceObject.Value._playerPositionBehaviour.updateInput(input);
            playerRefrenceObject.Value._playerSpellController.clientTriggerSpell(playerRefrenceObject.Value, input);
        }
    }
}
