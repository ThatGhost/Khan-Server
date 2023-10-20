using System.Collections.Generic;
using Khan_Shared.Magic;
using Khan_Shared.Utils;
using System.Linq;
using UnityEngine;
using PlayerSpellId = System.Int32;
using UnityEngine.Windows;

namespace Server.Services
{
    public class PlayerSpellController : IPlayerSpellController
    {
        private Dictionary<PlayerSpellId, PlayerSpell> m_playerSpells = new Dictionary<PlayerSpellId, PlayerSpell>();
        private Dictionary<int, PlayerSpellId> m_keyToSpells = new Dictionary<int, PlayerSpellId>();

        public Spell[] getSpells() => m_playerSpells.Values.Select(p => p.spell).ToArray();

        public void addSpell(PlayerSpell playerSpell, int key)
        {
            m_playerSpells.Add(playerSpell.playerSpellId, playerSpell);
            m_keyToSpells.Add(key, playerSpell.playerSpellId);
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

        public void clientTriggerSpell(PlayerRefrenceObject playerRefrence, SInput input)
        {
            ushort keys = input.keys;

            bool trigger1 = (keys & 256) > 0;
            bool trigger2 = (keys & 512) > 0;
            bool trigger3 = (keys & 1024) > 0;
            bool trigger4 = (keys & 2048) > 0;
            bool trigger5 = (keys & 4096) > 0;

            if(trigger1) m_playerSpells[m_keyToSpells[0]].spell.Trigger(new object[] { playerRefrence });
            if(trigger2) m_playerSpells[m_keyToSpells[1]].spell.Trigger(new object[] { playerRefrence });
            if(trigger3) m_playerSpells[m_keyToSpells[2]].spell.Trigger(new object[] { playerRefrence });
            if(trigger4) m_playerSpells[m_keyToSpells[3]].spell.Trigger(new object[] { playerRefrence });
            if(trigger5) m_playerSpells[m_keyToSpells[4]].spell.Trigger(new object[] { playerRefrence });
        }
    }
}
