using System;
using UnityEngine;
using Khan_Shared.Networking;
using Networking.Behaviours;
using Zenject;

namespace Networking.Services
{
    public class FooService: IFooService
    {
        [Inject] private readonly ITestBehaviour m_baseBehaviour;
        [Inject] private readonly IMessagePublisher m_messagePublisher;

        public void Foo(int data, int other, int conn)
        {
            Message msg = new Message(MessageTypes.HandShake, new object[2] { (uint)3, (uint)5 }, MessagePriorities.high, true);
            m_messagePublisher.PublishMessage(msg, conn);
            m_baseBehaviour.doSomething();
        }
    }
}