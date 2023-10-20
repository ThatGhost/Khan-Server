using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;
using Server.Behaviours;
using Zenject;
using Server.Services;

namespace Server.Magic
{
    public class PrefabBuilder_WWB : PrefabBuilder
    {
        private bool moving = false;
        private float speed = 0;

        private Rigidbody rb;

        // Modify the components based on spell params
        public override void build(Spell spell)
        {
            if (spell is not Spell_WaterBall waterBall) return;
            // initialize the spell specific components

            speed = waterBall.speed;
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!moving) return;
            rb.MovePosition(transform.forward * speed);
        }

        public override void start()
        {
            StartCoroutine(die(30));
            moving = true;
        }

        private IEnumerator die(float deathTime)
        {
            yield return new WaitForSeconds(deathTime);
            onDestruction.Invoke(this);
            moving = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            moving = false;
            onDestruction.Invoke(this);
        }
    }
}
