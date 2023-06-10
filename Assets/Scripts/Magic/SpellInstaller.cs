using Khan_Shared.Magic;
using Server.Magic;
using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SpellInstaller", menuName = "Installers/SpellInstaller")]
public class SpellInstaller : ScriptableObjectInstaller<SpellInstaller>
{
    public SpellModifierCombination fire_FireTower;

    public override void InstallBindings()
    {
        InstallUtillities();

        bindSpell(fire_FireTower);
    }

    private void InstallUtillities()
    {
        Container.Bind<ISpellClickUtillity>().To<SpellClickUtillity>().AsTransient();
        Container.Bind<ISpellNetworkingUtillity>().To<SpellNetworkingUtillity>().AsTransient();
        Container.Bind<ISpellPlayerUtillity>().To<SpellPlayerUtillity>().AsTransient();
        Container.Bind<ISpellPoolUtillity>().To<SpellPoolUtillity>().AsTransient();
    }


    // --Don't touch--
    private void bindSpell(SpellModifierCombination spell)
    {
        Container.Bind<Spell>().WithId(spell.spell.spellId).FromInstance(spell.spell);
        Container.Bind<GameObject>().FromInstance(spell.spell.prefab).WhenInjectedInto<SpellPoolUtillity>();
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