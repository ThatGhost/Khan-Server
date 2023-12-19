using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;
using Server.Behaviours;
using Zenject;
using Server.Services;

namespace Server.Magic
{
    public class PrefabBuilder_SSW : PrefabBuilder
    {
        // Modify the components based on spell params
        public override void build(Spell spell)
        {
            if (spell is not Spell_StoneWall stoneWall) return;
            // initialize the spell specific components


        }

        public override void start()
        {
            StartCoroutine(die(10));
        }

        private IEnumerator die(float deathTime)
        {
            yield return new WaitForSeconds(deathTime);
            onDestruction.Invoke(this);
        }
    }
}
