using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;
using Zenject;
using Networking.Services;
using Khan_Shared.Networking;

namespace Server.Magic
{
    [CreateAssetMenu(fileName = "FireTower", menuName = "Magic/Fire/FireTower/FireTower")]
    public class Spell_FireTower : Spell
    {
        public float size = 1;
        public Color color = Color.black;
        public float animationSpeed = 1;
        public float coolDownTime = 1;

        public bool enabled = true;

        [Inject] private readonly ILoggerService m_loggerService;
        [Inject] private readonly IMonoHelper m_monoHelper;
        [Inject] private readonly IClickTypeCalculator m_clickTypeCalculator;
        [Inject] private readonly IMessagePublisher m_messagePublisher;

        public override void Trigger()
        {
            if (enabled && m_clickTypeCalculator.isDown(true))
            {
                enabled = false;

                Instantiate(prefab);
                Message triggerSpellMessage = new Message(MessageTypes.SpellTrigger, new object[] { (ushort)connectionId, (ushort)playerSpellId } ,MessagePriorities.medium);
                m_messagePublisher.PublishGlobalMessage(triggerSpellMessage);

                m_monoHelper.StartCoroutine(coolDown());
            }
            m_clickTypeCalculator.clicked(true);
        }

        public override void Reset()
        {
            m_clickTypeCalculator.clicked(false);
        }

        private IEnumerator coolDown()
        {
            yield return new WaitForSecondsRealtime(coolDownTime);
            enabled = true;
        }
    }
}
