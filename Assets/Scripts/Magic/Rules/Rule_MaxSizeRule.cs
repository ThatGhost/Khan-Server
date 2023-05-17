using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Khan_Shared.Magic;

[CreateAssetMenu(fileName = "MaxSizeRule", menuName = "Magic/Rules/MaxSizeRule")]
public class Rule_MaxSizeRule : SpellRule
{
    public float maxSize = 10;

    public override void ApplyRule(SpellModifier modifier, Spell spell)
    {
        if(spell is Spell_FireTower fireTower)
        {
            if (fireTower.size > 20) fireTower.size = maxSize;
        }
    }
}
