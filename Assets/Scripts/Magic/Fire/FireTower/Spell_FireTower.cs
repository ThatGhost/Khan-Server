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
        public float timeUntilDamage = 9;
        public int damage = 10;
        public int manaCost = 10;

        public bool enabled = true;

        [Inject] private readonly IMonoHelper m_monoHelper;
        [Inject] private readonly ISpellPoolUtillity m_spellPoolUtillity;
        [Inject] private readonly ISpellNetworkingUtillity m_spellNetworkingUtillity;
        [Inject] private readonly ISpellPlayerUtillity m_spellPlayerUtillity;
        [Inject] private readonly IPlayersVariableService m_playersVariableService;

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

                m_monoHelper.StartCoroutine(WhaitToTrigger(metaData));
                m_monoHelper.StartCoroutine(coolDown());
            }
            else m_spellNetworkingUtillity.sendPostTrigger(playerSpellId, connectionId, false);
        }

        private void makeInstance(Vector3 position)
        {
            PrefabBuilder_FFT gameObject = m_spellPoolUtillity.request<PrefabBuilder_FFT>(this);
            gameObject.transform.position = position;

            gameObject.start();
        }

        private IEnumerator coolDown()
        {
            yield return new WaitForSeconds(cooldown + (timeToActivation / 60));
            enabled = true;
        }

        private IEnumerator WhaitToTrigger(object[] metaData)
        {
            yield return new WaitForSeconds(timeToActivation / 60);

            PlayerRefrenceObject player = (PlayerRefrenceObject)metaData[0];
            Vector3 placePoint = m_spellPlayerUtillity.getGroundPoint(player);
            if (placePoint != Vector3.zero)
            {
                makeInstance(placePoint);
                m_playersVariableService.routeMana(playerSpellId, -manaCost);
                m_spellNetworkingUtillity.sendAOETrigger(playerSpellId, connectionId, placePoint);
            }
        }

        public override void Destruct()
        {
            m_spellPoolUtillity.destruct();
        }
    }
}
