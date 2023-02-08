using System;
using System.Collections.Generic;
using Khan_Shared.Networking;
using Networking.Services;
using Zenject;

namespace Networking.EntryPoints
{
    public class TestEntryPoint : EntryPointBase, ITestEntryPoint
    {
        [Inject] private readonly IFooService m_SetupService;

        public TestEntryPoint()
        {
            p_messages = new MessageFunctionPair[]
            {
                new MessageFunctionPair(MessageTypes.HandShake, handShake),
            };
        }

        private void handShake(object[] data, int conn)
        {
            m_SetupService.Foo((int)(uint)data[0], (int)(uint)data[1], conn);
        }
    }
}
