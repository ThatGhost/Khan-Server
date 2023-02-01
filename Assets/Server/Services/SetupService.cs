using System;
using UnityEngine;
using Khan_Shared.Networking;
using Networking.Shared;

namespace Networking.Services
{
    public class SetupService : ServiceBase
    {
        private BehaviourBase m_baseBehaviour;
        public override void Init()
        {
            m_baseBehaviour = DIContainer.Instance.getBehaviour<BehaviourBase>();
        } 

        public void foo(int data, int other, int conn)
        {
            Debug.Log("foo");
            Message msg = new Message(MessageTypes.HandShake, new object[2] { 3, 5 });
            MessageQueue.publishMessage(msg, conn);
            m_baseBehaviour.doSomething();
        }
    }
}