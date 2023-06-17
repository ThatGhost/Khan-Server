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

        // TEMP until outside storage
        private SpellIds[] spellIds = new SpellIds[]{ SpellIds.Fire_FireTower, SpellIds.Air_AirDash };

        public void InitializeSpells(ConnectionId connection)
        {
            int key = 0;
            foreach (var spellId in spellIds)
            {
                Spell spell = makeSpellForConnection(connection, spellId);

                m_playerController.getPlayer(connection).Value._playerSpellController.addSpell(new PlayerSpell()
                {
                    spell = spell,
                    playerSpellId = spell.PlayerSpellId,
                }, key);
                key++;

                sendConnectionSpellToConnection(connection, spell);
                sendOtherSpellsToConnection(connection);
                sendConnectionSpellToOthers(connection, spell);
            }
        }

        private Spell makeSpellForConnection(ConnectionId connection, SpellIds spellId)
        {
            Spell spell = m_container.ResolveId<Spell>(spellId);

            Spell instanceOfSpell = (Spell)(m_monoHelper.Instantiate(spell));
            m_container.Inject(instanceOfSpell);

            instanceOfSpell.Initialize(connection, m_currentPlayerSpellId);
            m_currentPlayerSpellId++;
            return instanceOfSpell;
        }

        private void sendConnectionSpellToConnection(ConnectionId connection, Spell spell)
        {
            Message initializeSpellMessage = new Message(MessageTypes.InitializeSpell, new object[]
            {
                (ushort)connection,
                (ushort)spell.PlayerSpellId,
                (ushort)spell.spellId,
                (uint)0,
                (uint)0,
                (uint)0,
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
                        (uint)0,
                        (uint)0,
                        (uint)0,
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
                (uint)0,
                (uint)0,
                (uint)0,
            }, MessagePriorities.high, true);
            m_messagePublisher.PublishGlobalMessage(initializeSpellMessage);
        }
    }
}
