using System.Collections;
using System.Collections.Generic;
using Khan_Shared.Networking;
using Networking.Services;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Server.Magic
{
    public class SpellNetworkingUtillity : ISpellNetworkingUtillity
    {
        [Inject] private readonly IMessagePublisher m_messagePublisher;

        public void sendAbilityTrigger(int playerSpellId, int connectionId)
        {
            Message triggerSpellMessage = new Message(MessageTypes.AbilityTrigger
            , new object[]
            {
                (ushort)connectionId,
                (ushort)playerSpellId,
            }
            , MessagePriorities.medium);

            m_messagePublisher.PublishGlobalMessage(triggerSpellMessage);
        }

        public void sendAOETrigger(int playerSpellId, int connectionId, Vector3 position)
        {
            Message triggerSpellMessage = new Message(MessageTypes.AOETrigger
            , new object[]
            {
                (ushort)connectionId,
                (ushort)playerSpellId,
                (float)position.x,
                (float)position.y,
                (float)position.z,
            }
            , MessagePriorities.medium);

            m_messagePublisher.PublishGlobalMessage(triggerSpellMessage);
        }

        public void sendPreTrigger(int playerSpellId, int connectionId)
        {
            Message preTriggerSpellMessage = new Message(MessageTypes.PreSpellTrigger
            , new object[]
            {
                (ushort)connectionId,
                (ushort)playerSpellId,
            }
            , MessagePriorities.medium);
            m_messagePublisher.PublishGlobalMessage(preTriggerSpellMessage);
        }
    }
}
