using Khan_Shared.Magic;
using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SpellInstaller", menuName = "Installers/SpellInstaller")]
public class SpellInstaller : ScriptableObjectInstaller<SpellInstaller>
{
    public SpellModifierCombination fire_FireTower;

    public override void InstallBindings()
    {
        bindSpell(fire_FireTower);
    }




    // --Don't touch--
    private void bindSpell(SpellModifierCombination spell)
    {
        Container.Bind<Spell>().WithId(spell.spell.spellId).FromInstance(spell.spell);
        foreach (var modifier in spell.modifiers)
        {
            Container.Bind<SpellModifier>().WithId(modifier.modifierId).FromInstance(modifier);
        }
    }

    [Serializable]
    public struct SpellModifierCombination
    {
        public Spell spell;
        public SpellModifier[] modifiers;
    }
}