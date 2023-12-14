using Server.Behaviours;
using Server.Services;
using UnityEngine;
using Zenject;

public class MonoPlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerPositionBehaviour>().To<PlayerPositionBehaviour>().FromComponentOnRoot().AsSingle();
        Container.Bind<IPlayerSpellController>().To<PlayerSpellController>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerVariableService>().AsSingle();
    }
}