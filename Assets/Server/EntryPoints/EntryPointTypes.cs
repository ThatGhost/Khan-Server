﻿using System;
using Khan_Shared.Networking;

namespace Server.EntryPoints
{
    public interface ITestEntryPoint : IEntryPoint { };
    public interface IPlayerEntryPoint : IEntryPoint { };




    public interface IEntryPointRegistry
    {
        public MessageFunctionPair[] MessagePairs { get; }
        public void Init();
    }
}