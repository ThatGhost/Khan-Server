using Khan_Shared.Magic;
using UnityEngine;

namespace Server.Magic
{
    [CreateAssetMenu(fileName = "AAD_MorePowerfull", menuName = "Magic/Air/AirDash/Mods/AAD_MorePowerfull")]
    public class Modifier_AirDash_MorePowerfull: SpellModifier
    {
        public float forceMultiplier = 1;

        public override void ModifySpell(Spell spell)
        {
            if (spell is Spell_AirDash airDash)
            {
                airDash.force *= forceMultiplier;
            }
        }
    }
}
