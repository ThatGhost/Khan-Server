using System;
using Khan_Shared.Networking;
using Zenject;
using Networking.Services;

namespace Networking.EntryPoints
{
    public class PlayerEntryPoint: EntryPointBase, IPlayerEntryPoint
    {
        [Inject] private readonly IPlayerInputService m_playerInputService;
        public PlayerEntryPoint()
        {
            p_messages = new MessageFunctionPair[]
            {
                new MessageFunctionPair(MessageTypes.PositionData, onInput),
            };
        }

        private void onInput(object[] data, int connection)
        {
            m_playerInputService.ReceivePlayerInput((byte[])data[0], connection);
        }
    }
}
