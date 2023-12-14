using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;
using Server.Behaviours;
using Zenject;
using Server.Services;

namespace Server.Magic
{
    public class PrefabBuilder_FFT : PrefabBuilder
    {
        // Components
        private float totalSpellDuration;
        private float startingSpellDuration;
        private int damage;

        private List<GameObject> m_collidedPlayers = new List<GameObject>();

        // Modify the components based on spell params
        public override void build(Spell spell)
        {
            if (spell is not Spell_FireTower fireTower) return;
            // initialize the spell specific components

            damage = fireTower.damage;
            totalSpellDuration = fireTower.totalSpellDuration;
            startingSpellDuration = fireTower.startingSpellDuration;
        }

        public override void start()
        {
            StartCoroutine(die(12));
            StartCoroutine(timeTillDamage());
        }

        private IEnumerator timeTillDamage(float timePassed = 0)
        {
            const float damageTickRate = 0.5f;
            yield return new WaitForSeconds(startingSpellDuration);

            while (timePassed < totalSpellDuration - startingSpellDuration)
            {
                foreach (var player in m_collidedPlayers)
                {
                    if (player.transform.parent.TryGetComponent(out PlayerBehaviour playerBehaviour) && player.activeInHierarchy)
                    {
                        playerBehaviour.m_playerVariableService.addHealth(-damage);
                    }
                }
                yield return new WaitForSeconds(damageTickRate);
                timePassed += damageTickRate;
            }
        }

        private IEnumerator die(float deathTime)
        {
            yield return new WaitForSeconds(deathTime);
            m_collidedPlayers.Clear();
            onDestruction.Invoke(this);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.layer == 7)
                m_collidedPlayers.Add(collision.gameObject);
        }
        private void OnTriggerExit(Collider collision)
        {
            if (collision.gameObject.layer == 7 && m_collidedPlayers.Contains(collision.gameObject))
            {
                m_collidedPlayers.Remove(collision.gameObject);
            }
        }
    }
}
