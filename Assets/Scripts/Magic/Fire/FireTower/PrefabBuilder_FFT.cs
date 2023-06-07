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
            StartCoroutine(die(12));

            // gameObject.transform.position = new Vector3(0, 0, fireTower.size);
        }

        private IEnumerator die(float deathTime)
        {
            yield return new WaitForSeconds(deathTime);
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<PrefabBuilder_FFT> { }
    }
}
