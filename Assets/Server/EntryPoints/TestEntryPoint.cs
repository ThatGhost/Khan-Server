﻿using System;
using System.Collections.Generic;
using Khan_Shared.Networking;
using Server.Services;
using Zenject;

namespace Server.EntryPoints
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
            m_SetupService.Foo((uint)data[0], (byte[])data[1], conn);
        }
    }
}
