using System.Collections;
using System.Collections.Generic;
using Khan_Shared.Networking;
using Server.Services;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Server.Magic
{
    public class SpellNetworkingUtillity : ISpellNetworkingUtillity
    {
        [Inject] private readonly IMessagePublisher m_messagePublisher;

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

        public void sendPlacementTrigger(int playerSpellId, int connectionId, Vector3 position, Vector3 rotation)
        {
            Message triggerSpellMessage = new Message(MessageTypes.PlacementTrigger
            , new object[]
            {
                (ushort)connectionId,
                (ushort)playerSpellId,
                (float)position.x,
                (float)position.y,
                (float)position.z,
                (float)rotation.x,
                (float)rotation.y,
                (float)rotation.z,
            }
            , MessagePriorities.medium);

            m_messagePublisher.PublishGlobalMessage(triggerSpellMessage);
        }

        public void sendPostTrigger(int playerSpellId, int connectionId, bool wasValid)
        {
            Message preTriggerSpellMessage = new Message(MessageTypes.PostSpellTrigger
            , new object[]
            {
                (ushort)connectionId,
                (ushort)playerSpellId,
                (bool)wasValid,
            }
            , MessagePriorities.medium, true);
            m_messagePublisher.PublishGlobalMessage(preTriggerSpellMessage);
        }
    }
}
