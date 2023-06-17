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

                Vector3 lookDirection = m_spellPlayerUtillity.getLookDirection(player);
                player._playerPositionBehaviour.AddForce(lookDirection * force);
                player._playerVariableService.addMana(-manaCost);

                m_monoHelper.StartCoroutine(coolDown());
                m_spellNetworkingUtillity.sendPreTrigger(playerSpellId, connectionId);
                m_spellNetworkingUtillity.sendAbilityTrigger(playerSpellId, connectionId);
            }
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
