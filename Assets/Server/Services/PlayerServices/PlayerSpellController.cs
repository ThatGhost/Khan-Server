using System.Collections.Generic;
using Khan_Shared.Magic;
using Khan_Shared.Utils;
using System.Linq;
using UnityEngine;
using Key = System.Int32;
using UnityEngine.Windows;

namespace Networking.Services
{
    public class PlayerSpellController : IPlayerSpellController
    {
        private Dictionary<Key, PlayerSpell> m_playerSpells = new Dictionary<Key, PlayerSpell>();

        public Spell[] getSpells() => m_playerSpells.Values.Select(p => p.spell).ToArray();

        public void receiveInput(PlayerRefrenceObject playerRefrence, SInput input)
        {
            bool trigger1 = trigger1 = (input.keys & 256) > 0;
            //bool trigger2 = false;
            //bool trigger4 = false;
            //bool trigger5 = false;

            if (trigger1) m_playerSpells[0].spell.Trigger(new object[] { playerRefrence });
            else m_playerSpells[0].spell.Reset();

            //if (trigger2) m_playerSpells[1].spell.Trigger();
            //else m_playerSpells[1].spell.Reset();
        }

        public void addSpell(Key key, PlayerSpell playerSpell)
        {
            m_playerSpells.Add(key, playerSpell);
        }

        public bool playerOwnsSpell(int playerSpellId)
        {
            foreach (var key in m_playerSpells.Values)
            {
                if (key.playerSpellId == playerSpellId) return true;
            }
            return false;
        }
    }
}
