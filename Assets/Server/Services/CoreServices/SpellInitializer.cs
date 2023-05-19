using Zenject;
using Khan_Shared.Magic;
using Khan_Shared.Networking;
using System.Linq;

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
            Spell spell = makeSpellForConnection(connection);

            m_playerController.getPlayer(connection).Value._playerSpellController.addSpell(0, new PlayerSpell()
            {
                spell = spell,
                playerSpellId = m_currentPlayerSpellId,
            });

            sendConnectionSpellToConnection(connection, spell);
            sendOtherSpellsToConnection(connection);
            sendConnectionSpellToOthers(connection, spell);
        }

        private Spell makeSpellForConnection(ConnectionId connection)
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
            m_currentPlayerSpellId++;
            return instanceOfSpell;
            // -- TEMP
        }

        private void sendConnectionSpellToConnection(ConnectionId connection, Spell spell)
        {
            Message initializeSpellMessage = new Message(MessageTypes.InitializeSpell, new object[]
            {
                (ushort)connection,
                (ushort)spell.PlayerSpellId,
                (ushort)spell.spellId,
                (uint)spell.AppliedModifiers[0],
                (uint)spell.AppliedModifiers[1],
                (uint)spell.AppliedModifiers[2],
            }, MessagePriorities.high, true);
            m_messagePublisher.PublishMessage(initializeSpellMessage, connection);
        }

        private void sendOtherSpellsToConnection(ConnectionId connection)
        {
            PlayerRefrenceObject[] players = m_playerController.getPlayers();
            foreach (var player in players.Where(p => p._connectionId != connection))
            {
                Spell[] spells = player._playerSpellController.getSpells();
                foreach (var spell in spells)
                {
                    Message initializeSpellMessage = new Message(MessageTypes.InitializeSpell, new object[]
                    {
                        (ushort)player._connectionId,
                        (ushort)spell.PlayerSpellId,
                        (ushort)spell.spellId,
                        (uint)spell.AppliedModifiers[0],
                        (uint)spell.AppliedModifiers[1],
                        (uint)spell.AppliedModifiers[2],
                    }, MessagePriorities.high, true);
                    m_messagePublisher.PublishMessage(initializeSpellMessage, connection);
                }
            }
        }

        private void sendConnectionSpellToOthers(ConnectionId connection, Spell spell)
        {
            Message initializeSpellMessage = new Message(MessageTypes.InitializeSpell, new object[]
            {
                (ushort)connection,
                (ushort)spell.PlayerSpellId,
                (ushort)spell.spellId,
                (uint)spell.AppliedModifiers[0],
                (uint)spell.AppliedModifiers[1],
                (uint)spell.AppliedModifiers[2],
            }, MessagePriorities.high, true);
            m_messagePublisher.PublishGlobalMessage(initializeSpellMessage);
        }
    }
}
