using System.Collections.Generic;
using Khan_Shared.Magic;
using Khan_Shared.Utils;
using System.Linq;
using UnityEngine;
using Key = System.Int32;

namespace Networking.Services
{
    public class PlayerSpellController : IPlayerSpellController
    {
        private Dictionary<Key, PlayerSpell> m_playerSpells = new Dictionary<Key, PlayerSpell>();

        public Spell[] getSpells() => m_playerSpells.Values.Select(p => p.spell).ToArray();

        public void receiveInput(PlayerRefrenceObject playerRefrence, SInput[] inputs)
        {
            bool trigger1 = false;
            //bool trigger2 = false;
            //bool trigger4 = false;
            //bool trigger5 = false;

            foreach (var input in inputs)   
            {
                if(!trigger1) trigger1 = (input.keys & 256) > 0;  // 0000 0001 0000 0000
                //if(!trigger2) trigger2 = (input.keys & 512) > 0;  // 0000 0010 0000 0000
                //if(!trigger3) trigger3 = (input.keys & 1024) > 0; // 0000 0100 0000 0000
                //if(!trigger4) trigger4 = (input.keys & 2048) > 0; // 0000 1000 0000 0000
            }

            if (trigger1) m_playerSpells[0].spell.Trigger(new object[] { playerRefrence });
            else m_playerSpells[0].spell.Reset();

            //if (trigger2) m_playerSpells[1].spell.Trigger();
            //else m_playerSpells[1].spell.Reset();
        }

        public void addSpell(Key key, PlayerSpell playerSpell)
        {
            m_playerSpells.Add(key, playerSpell);
        }

    }
}
