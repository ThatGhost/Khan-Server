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
        [Inject] private readonly IPlayersVariableService m_playersVariableService;

        // Components
        private float timeUntilDamage;
        private int playerSpellId;
        private int damage;

        private List<GameObject> m_collidedPlayers = new List<GameObject>();

        // Modify the components based on spell params
        public override void build(Spell spell)
        {
            playerSpellId = spell.PlayerSpellId;
            if (spell is not Spell_FireTower fireTower) return;
            // initialize the spell specific components

            damage = fireTower.damage;
            timeUntilDamage = fireTower.timeUntilDamage;
        }

        public override void start()
        {
            StartCoroutine(die(12));
            StartCoroutine(timeTillDamage());
        }

        private IEnumerator timeTillDamage()
        {
            yield return new WaitForSeconds(timeUntilDamage);
            m_playersVariableService.routeHealth(m_collidedPlayers.ToArray(), -damage, playerSpellId);
        }

        private IEnumerator die(float deathTime)
        {
            yield return new WaitForSeconds(deathTime);
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
