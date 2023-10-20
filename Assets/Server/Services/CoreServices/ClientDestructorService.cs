using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Khan_Shared.Networking;
using ConnectionId = System.Int32;

namespace Server.Services
{
    public class ClientDestructorService : IClientDestructorService
    {
        [Inject] private readonly IPlayersController m_playersController;
        [Inject] private readonly IMessagePublisher m_messagePublisher;
        [Inject] private readonly IMonoHelper m_monoHelper;

        public void DestructClient(ConnectionId connectionId)
        {
            PlayerRefrenceObject? player = m_playersController.getPlayer(connectionId);
            if (player == null) return;

            player.Value._playerSpellController.destructSpells();
            m_monoHelper.Destroy(player.Value._gameObject);
            m_playersController.removePlayer(connectionId);

            sendLeaveMessage(connectionId);
        }

        private void sendLeaveMessage(ConnectionId connectionId)
        {
            Message msg = new Message(MessageTypes.PlayerLeave, new object[]
            {
                (ushort) connectionId
            }, MessagePriorities.high, true);
            m_messagePublisher.PublishGlobalMessage(msg);
        }
    }
}
