﻿using System;
using UnityEngine;
using Khan_Shared.Networking;
using Networking.Shared;
using Networking.Behaviours;
using Zenject;

namespace Networking.Services
{
    public class FooService: IFooService
    {
        [Inject] private readonly ITestBehaviour m_baseBehaviour;

        public void Foo(int data, int other, int conn)
        {
            Debug.Log("foo");
            Message msg = new Message(MessageTypes.HandShake, new object[2] { 3, 5 });
            //MessageQueue.publishMessage(msg, conn);
            m_baseBehaviour.doSomething();
        }
    }
}