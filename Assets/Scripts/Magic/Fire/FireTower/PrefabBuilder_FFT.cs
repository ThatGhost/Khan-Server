using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;
using Networking.Behaviours;
using Zenject;

namespace Server.Magic
{
    public class PrefabBuilder_FFT : PrefabBuilder
    {
        // Components

        // Modify the components based on spell params
        public override void build(Spell spell)
        {
            if (spell is not Spell_FireTower fireTower) return;

            // initialize the spell specific components
        }

        public override void start()
        {
            StartCoroutine(die(12));
        }

        private IEnumerator die(float deathTime)
        {
            yield return new WaitForSeconds(deathTime);
            onDestruction.Invoke(this);
        }
    }
}
