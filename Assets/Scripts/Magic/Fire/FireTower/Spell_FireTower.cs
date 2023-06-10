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

        public bool enabled = true;

        [Inject] private readonly IMonoHelper m_monoHelper;
        [Inject] private readonly ISpellPoolUtillity m_spellPoolUtillity;
        [Inject] private readonly ISpellNetworkingUtillity m_spellNetworkingUtillity;
        [Inject] private readonly ISpellPlayerUtillity m_spellPlayerUtillity;

        public override void Initialize(int connectionId, int playerSpellId, int key)
        {
            base.Initialize(connectionId, playerSpellId, key);
            m_spellPoolUtillity.setup(this.prefab);
        }

        public override void Trigger(object[] metaData)
        {
            if (enabled)
            {
                enabled = false;

                m_spellNetworkingUtillity.sendPreTrigger(playerSpellId, connectionId);

                m_monoHelper.StartCoroutine(WhaitToTrigger(metaData));
                m_monoHelper.StartCoroutine(coolDown());
            }
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
            Vector3 placePoint = m_spellPlayerUtillity.getPlacementPoint(player, false);
            if (placePoint != Vector3.zero)
            {
                makeInstance(placePoint);
                m_spellNetworkingUtillity.sendAOETrigger(playerSpellId, connectionId, placePoint);
            }
        }
    }
}
