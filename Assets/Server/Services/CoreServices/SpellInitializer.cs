using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Khan_Shared.Magic;
using Khan_Shared.Networking;

using ConnectionId = System.Int32;

namespace Networking.Services
{
    public class SpellInitializer : ISpellInitializer
    {
        [Inject] private readonly DiContainer m_container;
        [Inject] private readonly IPlayersController m_playerController;
        [Inject] private readonly IMessagePublisher m_messagePublisher;
        [Inject] private readonly IMonoHelper m_monoHelper;

        private int m_currentPlayerSpellId = 1;

        private SpellIds spellId = SpellIds.Fire_FireTower;
        private SpellModifierIds[] modifierIds = new SpellModifierIds[]
        {
            SpellModifierIds.FireTower_MakeBigger,
            SpellModifierIds.FireTower_MakeBigger,
            SpellModifierIds.FireTower_MakeBigger,
        };

        public void InitializeSpells(ConnectionId connection)
        {
            // TEMP -- until outside spell storage per player
            Spell spell = m_container.ResolveId<Spell>(spellId);

            Spell instanceOfSpell = (Spell)(m_monoHelper.Instantiate(spell));
            m_container.Inject(instanceOfSpell);

            foreach (var modifierId in modifierIds)
            {
                SpellModifier modifier = m_container.ResolveId<SpellModifier>(modifierId);
                instanceOfSpell.ApplyModifiers(modifier);
            }
            instanceOfSpell.Initialize(connection, m_currentPlayerSpellId);

            m_playerController.getPlayer(connection).Value._playerSpellController.addSpell(0, new PlayerSpell()
            {
                spell = instanceOfSpell,
                playerSpellId = m_currentPlayerSpellId,
            });

            Message initializeSpellMessage = new Message(MessageTypes.InitializeSpell, new object[]
            {
                (ushort)connection,
                (ushort)m_currentPlayerSpellId,
                (ushort)spellId,
                (uint)modifierIds[0],
                (uint)modifierIds[1],
                (uint)modifierIds[2],
            } ,MessagePriorities.high, true);
            m_messagePublisher.PublishGlobalMessage(initializeSpellMessage);
            m_currentPlayerSpellId++;
            // -- TEMP
        }
    }
}
