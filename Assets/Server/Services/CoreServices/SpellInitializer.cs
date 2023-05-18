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
            Spell spell = m_container.ResolveId<Spell>(spellId);
            foreach (var modifierId in modifierIds)
            {
                SpellModifier modifier = m_container.ResolveId<SpellModifier>(modifierId);
                modifier.ModifySpell(spell);
            }

            m_playerController.getPlayer(connection).Value._playerSpellController.addSpell(spell, m_currentPlayerSpellId);

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
        }
    }
}
