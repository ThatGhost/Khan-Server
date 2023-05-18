using System.Collections;
using System.Collections.Generic;
using Khan_Shared.Magic;
using Networking.Services;
using UnityEngine;
using PlayerSpellId = System.Int32;
using ConnectionId = System.Int32;

namespace Networking.Services
{
    public class PlayerSpellController : IPlayerSpellController
    {
        private Dictionary<PlayerSpellId, Spell> m_playerSpells = new Dictionary<PlayerSpellId, Spell>();

        public void addSpell(Spell spell, PlayerSpellId playerSpellId)
        {
            m_playerSpells.Add(playerSpellId, spell);
        }

        public void triggerSpell(PlayerSpellId playerSpellId)
        {
            m_playerSpells[playerSpellId].Trigger();
        }
    }
}
