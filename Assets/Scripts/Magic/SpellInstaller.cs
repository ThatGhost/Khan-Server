using Khan_Shared.Magic;
using Server.Magic;
using System;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SpellInstaller", menuName = "Installers/SpellInstaller")]
public class SpellInstaller : ScriptableObjectInstaller<SpellInstaller>
{
    public SpellModifierCombination fire_FireTower;
    public SpellModifierCombination air_AirDash;
    public SpellModifierCombination stone_StoneWall;
    public SpellModifierCombination water_WaterBall;

    public override void InstallBindings()
    {
        InstallUtillities();

        bindSpell(fire_FireTower);
        bindSpell(air_AirDash);
        bindSpell(stone_StoneWall);
        bindSpell(water_WaterBall);
    }

    private void InstallUtillities()
    {
        Container.Bind<ISpellClickUtillity>().To<SpellClickUtillity>().AsTransient();
        Container.Bind<ISpellNetworkingUtillity>().To<SpellNetworkingUtillity>().AsTransient();
        Container.Bind<ISpellPlayerUtillity>().To<SpellPlayerUtillity>().AsTransient();
        Container.Bind<ISpellPoolUtillity>().To<SpellPoolUtillity>().AsTransient();
        Container.BindInstance(new Vector3(-0.555f, -0.318f, 1.098f)).WithId(FirePositions.FirePosition1).AsSingle();
        Container.BindInstance(new Vector3(0.555f, -0.318f, 1.098f)).WithId(FirePositions.FirePosition2).AsSingle();
        Container.BindInstance(new Vector3(0f, -0.494f, 1.098f)).WithId(FirePositions.FirePosition3).AsSingle();
    }


    // -- Don't touch --
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

public static class FirePositions
{
    public const string FirePosition1 = "fireposition_1";
    public const string FirePosition2 = "fireposition_2";
    public const string FirePosition3 = "fireposition_3";
}
