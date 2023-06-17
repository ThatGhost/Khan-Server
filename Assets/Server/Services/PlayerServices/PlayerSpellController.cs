using System.Collections.Generic;
using Khan_Shared.Magic;
using Khan_Shared.Utils;
using System.Linq;
using UnityEngine;
using PlayerSpellId = System.Int32;
using UnityEngine.Windows;

namespace Networking.Services
{
    public class PlayerSpellController : IPlayerSpellController
    {
        private Dictionary<PlayerSpellId, PlayerSpell> m_playerSpells = new Dictionary<PlayerSpellId, PlayerSpell>();
        private Dictionary<int, PlayerSpellId> m_keyToPlayerSpellId = new Dictionary<int, PlayerSpellId>();

        public Spell[] getSpells() => m_playerSpells.Values.Select(p => p.spell).ToArray();

        public void receiveInput(PlayerRefrenceObject playerRefrence, SInput input)
        {
            bool trigger1 = (input.keys & 256) > 0;
            bool trigger2 = (input.keys & 512) > 0;
            //bool trigger4 = false;
            //bool trigger5 = false;

            if (trigger1) m_playerSpells[m_keyToPlayerSpellId[0]].spell.Trigger(new object[] { playerRefrence });
            else m_playerSpells[m_keyToPlayerSpellId[0]].spell.Reset();

            if (trigger2) m_playerSpells[m_keyToPlayerSpellId[1]].spell.Trigger(new object[] { playerRefrence });
            else m_playerSpells[m_keyToPlayerSpellId[1]].spell.Reset();
        }

        public void addSpell(PlayerSpell playerSpell, int key)
        {
            m_playerSpells.Add(playerSpell.playerSpellId, playerSpell);
            m_keyToPlayerSpellId.Add(key, playerSpell.playerSpellId);
        }

        public bool playerOwnsSpell(int playerSpellId)
        {
            foreach (var key in m_playerSpells.Values)
            {
                if (key.playerSpellId == playerSpellId) return true;
            }
            return false;
        }

        public void destructSpells()
        {
            foreach (var spell in m_playerSpells)
            {
                spell.Value.spell.Destruct();
            }
        }
    }
}
