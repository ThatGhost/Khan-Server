using System;
using System.Collections;
using System.Collections.Generic;
using Khan_Shared.Networking;
using Networking.Behaviours;
using Networking.Core;
using Zenject;

using UnityEngine;

namespace Networking.Services
{
    public class FooService : IFooService
    {
        [Inject] private readonly ITestBehaviour m_baseBehaviour;
        [Inject] private readonly IMessagePublisher m_messagePublisher;

        public FooService() {
            GameServer.onClientConnect += onconnection;
        }

        public void Foo(uint data, byte[] other, int conn)
        {
            ushort datasize = BitConverter.ToUInt16(new byte[2] { other[0], other[1] });
            ushort datasend = BitConverter.ToUInt16(new byte[2] { other[2], other[3] });
            Debug.Log($"datasize {datasize}, datasend {datasend}");

            m_baseBehaviour.doSomething();
        }

        private void onconnection(int connection)
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes((ushort)2));
            bytes.AddRange(BitConverter.GetBytes((ushort)7));

            Message msg = new Message(MessageTypes.HandShake, new object[2] { (uint)3, (byte[])bytes.ToArray() }, MessagePriorities.high);
            m_messagePublisher.PublishMessage(msg, connection);
        }
    }
}