using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;

namespace Server.Magic
{
    [CreateAssetMenu(fileName = "MaxPowerRule", menuName = "Magic/Rules/MaxPowerRule")]
    public class Rule_MaxPowerRule : SpellRule
    {
        public float maxPower = 10;

        public override void ApplyRule(SpellModifier modifier, Spell spell)
        {
            if (spell is Spell_AirDash airDash)
            {
                if (airDash.force > maxPower) airDash.force = maxPower;
            }
        }
    }
}
