using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;
using Zenject;
using Server.Services;
using Khan_Shared.Networking;

namespace Server.Magic
{
    [CreateAssetMenu(fileName = "Spell_WaterBall", menuName = "Magic/Water/WaterBall/WaterBall")]
    public class Spell_WaterBall : Spell
    {
        public float size = 1;
        public int manaCost = 5;
        public float speed = 5;

        public bool enabled = true;

        [Inject] private readonly IMonoHelper m_monoHelper;
        [Inject] private readonly ISpellPoolUtillity m_spellPoolUtillity;
        [Inject] private readonly ISpellNetworkingUtillity m_spellNetworkingUtillity;
        [Inject] private readonly ISpellPlayerUtillity m_spellPlayerUtillity;
        [Inject] private readonly IPlayersVariableService m_playersVariableService;
        [Inject(Id = FirePositions.FirePosition1)] public Vector3 firePosition;

        public override void Initialize(int connectionId, int playerSpellId)
        {
            base.Initialize(connectionId, playerSpellId);
            m_spellPoolUtillity.setup(this.prefab);
        }

        public override void Trigger(object[] metaData)
        {
            PlayerRefrenceObject player = (PlayerRefrenceObject)metaData[0];

            if (enabled && player._playerVariableService.Mana >= manaCost)
            {
                enabled = false;

                m_spellNetworkingUtillity.sendPostTrigger(playerSpellId, connectionId, true);

                makeInstance((PlayerRefrenceObject)metaData[0]);
                m_monoHelper.StartCoroutine(coolDown());
                m_playersVariableService.routeMana(playerSpellId, -manaCost);
            }
            else m_spellNetworkingUtillity.sendPostTrigger(playerSpellId, connectionId, false);
        }

        private void makeInstance(PlayerRefrenceObject playerRefrenceObject)
        {
            PrefabBuilder_WWB gameObject = m_spellPoolUtillity.request<PrefabBuilder_WWB>(this);
            Vector3 lookdirection = m_spellPlayerUtillity.getLookDirection(playerRefrenceObject, true);
            gameObject.transform.localRotation = Quaternion.Euler(lookdirection.x, lookdirection.y, lookdirection.z);

            lookdirection = m_spellPlayerUtillity.getLookDirection(playerRefrenceObject, false);
            Vector3 relativeFirePosition = new Vector3(Mathf.Cos(lookdirection.x) * firePosition.x, 0, Mathf.Cos(lookdirection.y) * firePosition.z);
            gameObject.transform.localPosition = playerRefrenceObject._playerPositionBehaviour.Face.position + relativeFirePosition;

            gameObject.start();
        }

        private IEnumerator coolDown()
        {
            yield return new WaitForSeconds(cooldown + (timeToActivation / 60));
            enabled = true;
        }

        public override void Destruct()
        {
            m_spellPoolUtillity.destruct();
        }
    }
}
