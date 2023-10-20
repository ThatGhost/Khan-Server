using System;
using System.Linq;
using System.Collections.Generic;
using Khan_Shared.Networking;
using Khan_Shared.Utils;
using Zenject;
using Server.Services;

using UnityEngine;

namespace Server.EntryPoints
{
    public class PlayerEntryPoint: EntryPointBase, IPlayerEntryPoint
    {
        [Inject] private readonly IPlayerInputService m_playerInputService;

        public PlayerEntryPoint()
        {
            p_messages = new MessageFunctionPair[]
            {
                new MessageFunctionPair(MessageTypes.InputData, onInput),
            };
        }

        private void onInput(object[] data, int connection)
        {
            SInput input = new SInput();
            input.keys = (ushort)data[0];
            input.x = (float)data[1];
            input.y = (float)data[2];

            m_playerInputService.ReceivePlayerInput(input, connection);
        }
    }
}
