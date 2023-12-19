using System.Collections;
using UnityEngine;
using Khan_Shared.Magic;
using Zenject;
using Server.Services;

namespace Server.Magic
{
    [CreateAssetMenu(fileName = "Spell_StoneWall", menuName = "Magic/Stone/StoneWall/StoneWall")]
    public class Spell_StoneWall : Spell
    {
        public float size = 1;
        public int manaCost = 10;

        public bool enabled = true;

        [Inject] private readonly IMonoHelper m_monoHelper;
        [Inject] private readonly ISpellPoolUtillity m_spellPoolUtillity;
        [Inject] private readonly ISpellNetworkingUtillity m_spellNetworkingUtillity;
        [Inject] private readonly ISpellPlayerUtillity m_spellPlayerUtillity;
        [Inject] private readonly SignalBus m_signalBus;

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

                Vector3 placePoint = m_spellPlayerUtillity.getGroundPoint(player);
                Vector3 direction = m_spellPlayerUtillity.getLookDirection(player, true);
                direction = new Vector3(0, direction.y + 90, 0);

                if (placePoint != Vector3.zero)
                {
                    makeInstance(placePoint, direction);
                    m_signalBus.Fire(new OnManaSignal() { connectionId = this.connectionId, amount = -manaCost });
                    m_spellNetworkingUtillity.sendPlacementTrigger(playerSpellId, connectionId, placePoint, direction);
                    m_spellNetworkingUtillity.sendPostTrigger(playerSpellId, connectionId, true);
                }
                else m_spellNetworkingUtillity.sendPostTrigger(playerSpellId, connectionId, false);

                m_monoHelper.StartCoroutine(coolDown());
            }
            else m_spellNetworkingUtillity.sendPostTrigger(playerSpellId, connectionId, false);
        }

        private void makeInstance(Vector3 position, Vector3 direction)
        {
            PrefabBuilder_SSW gameObject = m_spellPoolUtillity.request<PrefabBuilder_SSW>(this);
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = Quaternion.Euler(direction);

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
