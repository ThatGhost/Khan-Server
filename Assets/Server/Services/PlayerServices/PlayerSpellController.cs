using System.Collections.Generic;
using Khan_Shared.Magic;
using Khan_Shared.Simulation;
using System.Linq;
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

            foreach (var input in inputs)   
            {
                trigger1 = (input.keys & 16) > 0;  // 0001 0000
                //trigger2 = (input.keys & 32) > 0;  // 0010 0000
                //trigger2 = (input.keys & 64) > 0;  // 0100 0000
                //trigger2 = (input.keys & 128) > 0; // 1000 0000
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
