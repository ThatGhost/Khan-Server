using Networking.Behaviours;
using Networking.Services;
using UnityEngine;
using Zenject;

public class MonoPlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerPositionBehaviour>().To<PlayerPositionBehaviour>().FromComponentOnRoot().AsSingle();
        Container.Bind<IPlayerSpellController>().To<PlayerSpellController>().AsSingle();
        Container.Bind<IPlayerVariableService>().To<PlayerVariableService>().AsSingle();
    }
}