using Networking.Behaviours;
using UnityEngine;
using Zenject;

public class MonoPlayerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPlayerPositionBehaviour>().To<PlayerPositionBehaviour>().FromComponentOnRoot().AsSingle();
    }
}