using UnityEngine;
using System.Collections;
using Zenject;
using Networking.Services;
using Khan_Shared.Networking;

namespace Networking.Behaviours
{
    public class TestBehaviour : MonoBehaviour
    {
        [Inject] private readonly Spell_Fire_FlameTower.Factory m_factory;
        [Inject] private readonly IMessagePublisher m_messagePublisher;
        [Inject] private Transform g_root;

        private void Update()
        {
            if (Input.GetKeyDown("c"))
            {
                Spell_Fire_FlameTower flameTower = m_factory.Create();
                flameTower.gameObject.transform.SetParent(g_root);
                flameTower.gameObject.transform.position = Vector3.zero;

                Message msg = new Message(MessageTypes.AOESpell, new object[] { (ushort)(1), 0f, 0f, 0f }, MessagePriorities.medium);
                m_messagePublisher.PublishGlobalMessage(msg);
            }
        }
    }
}