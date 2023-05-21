using System;
using System.Linq;
using System.Collections.Generic;
using Khan_Shared.Networking;
using Khan_Shared.Utils;
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
            for (int i = 0; i < usableData.Length; i += 10)
            {
                ushort keys = BitConverter.ToUInt16(new byte[2] { usableData[i + 0], usableData[i + 1] });
                float x = BitConverter.ToSingle(new byte[4] { usableData[i + 2], usableData[i + 3], usableData[i + 4], usableData[i + 5] });
                float y = BitConverter.ToSingle(new byte[4] { usableData[i + 6], usableData[i + 7], usableData[i + 8], usableData[i + 9] });

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
