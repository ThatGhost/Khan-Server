using Khan_Shared.Magic;
using UnityEngine;

[CreateAssetMenu(fileName = "FFT_MakeBiggerMod", menuName = "Magic/Fire/FireTower/Mods/FFT_MakeBiggerMod")]
public class Modifier_FireTower_MakeBigger : SpellModifier
{
    public float sizeMultiplier = 1;

    public override void ModifySpell(Spell spell)
    {
        if(spell is Spell_FireTower fireTower)
        {
            fireTower.size *= sizeMultiplier;
        }
    }
}
