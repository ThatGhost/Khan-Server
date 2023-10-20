using System.Collections;
using UnityEngine;
using Khan_Shared.Magic;
using Zenject;
using Networking.Services;
using Khan_Shared.Networking;

namespace Server.Magic
{
    [CreateAssetMenu(fileName = "AirDash", menuName = "Magic/Air/AirDash/AirDash")]
    public class Spell_AirDash : Spell
    {
        public int manaCost = 10;
        public float force = 10;

        public bool enabled = true;

        [Inject] private readonly IMonoHelper m_monoHelper;
        [Inject] private readonly ISpellNetworkingUtillity m_spellNetworkingUtillity;
        [Inject] private readonly ISpellPlayerUtillity m_spellPlayerUtillity;

        public override void Trigger(object[] metaData)
        {
            PlayerRefrenceObject player = (PlayerRefrenceObject)metaData[0];

            if (enabled && player._playerVariableService.Mana >= manaCost)
            {
                enabled = false;

                Vector3 lookDirection = m_spellPlayerUtillity.getLookDirection(player, false);
                lookDirection.y = 0;
                player._playerPositionBehaviour.AddForce(lookDirection * force);

                m_spellNetworkingUtillity.sendPostTrigger(playerSpellId, connectionId, true);
                m_monoHelper.StartCoroutine(coolDown());
            }
            else m_spellNetworkingUtillity.sendPostTrigger(playerSpellId, connectionId, false);
        }

        private IEnumerator coolDown()
        {
            yield return new WaitForSeconds(cooldown + (timeToActivation / 60));
            enabled = true;
        }

        public override void Destruct()
        {
            // nothing to destruct
        }
    }
}
