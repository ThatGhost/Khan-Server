using System;
using System.Linq;
using System.Collections.Generic;
using Khan_Shared.Networking;
using Khan_Shared.Simulation;
using Zenject;
using Networking.Services;

using UnityEngine;

namespace Networking.EntryPoints
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
            byte[] usableData = ((byte[])data[0]).Skip(2).ToArray();
            List<SInput> inputs = new List<SInput>();
            for (int i = 0; i < usableData.Length; i += 5)
            {
                byte keys = usableData[i + 0];
                short x = BitConverter.ToInt16(new byte[2] { usableData[i + 1], usableData[i + 2] });
                short y = BitConverter.ToInt16(new byte[2] { usableData[i + 3], usableData[i + 4] });

                inputs.Add(new SInput()
                {
                    keys = keys,
                    x = x,
                    y = y
                });
            }

            m_playerInputService.ReceivePlayerInput(inputs.ToArray(), connection);
        }
    }
}
