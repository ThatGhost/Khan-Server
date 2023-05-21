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
        [Inject] private readonly PrefabBuilder_FFT.Factory m_prefabFactory;

        public override void Trigger(object[] metaData)
        {
            if (enabled && m_clickTypeCalculator.isDown(true))
            {
                PlayerRefrenceObject player = (PlayerRefrenceObject)metaData[0];
                Vector3 placePoint = getPlacePoint(player);
                if (placePoint == Vector3.zero) return;

                makeInstance(placePoint);
                sendTrigger(placePoint);

                enabled = false;
                m_monoHelper.StartCoroutine(coolDown());
            }
            m_clickTypeCalculator.clicked(true);
        }

        private Vector3 getPlacePoint(PlayerRefrenceObject player)
        {
            Transform face = player._playerPositionBehaviour.Face;
            int layerMask = 1 << 6; // spells can't interact with spells
            layerMask = ~layerMask;

            Vector3 position = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(face.transform.position, face.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                return hit.point;
            }
            return Vector3.zero;
        }

        private void makeInstance(Vector3 position)
        {
            PrefabBuilder_FFT gameObject = m_prefabFactory.Create();
            gameObject.transform.position = position;
            gameObject.build(this);
        }

        private void sendTrigger(Vector3 position)
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
