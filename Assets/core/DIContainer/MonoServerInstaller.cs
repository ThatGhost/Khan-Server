using Networking.Core;
using Networking.Services;
using Networking.Behaviours;
using Networking.EntryPoints;

using UnityEngine;
using Zenject;

public class MonoServerInstaller : MonoInstaller
{
    public GameObject server;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameServer>().AsSingle().NonLazy();
        Container.Bind<ICoder>().To<Coder>().AsSingle();
        Container.Bind<IMessageQueue>().To<MessageQueue>().AsSingle();
        Container.Bind<IEntryPointRegistry>().To<EntryPointRegistry>().AsSingle();

        registerEntryPoints();
        registerServices();
        registerBehaviours();
    }

    private void registerEntryPoints()
    {
        //Container.Bind<ITestEntryPoint>().To<TestEntryPoint>().AsSingle();
        Container.Bind<IPlayerEntryPoint>().To<PlayerEntryPoint>().AsSingle();
    }

    private void registerServices()
    {
        //Container.Bind<IFooService>().To<FooService>().AsSingle();
        Container.Bind<IMonoHelper>().To<MonoHelpers>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IMessagePublisher>().To<MessagePublisher>().AsSingle();
        Container.Bind<Networking.Services.ILogger>().To<Networking.Services.Logger>().AsSingle();
        Container.Bind<IPlayerInputService>().To<PlayerInputService>().AsSingle();
    }

    private void registerBehaviours()
    {
        //Container.Bind<ITestBehaviour>().To<TestBehaviour>().FromComponentOn(server).AsSingle();
        Container.Bind<IPlayerPositionBehaviour>().To<PlayerPositionBehaviour>().FromComponentOn(server).AsSingle();
    }
}

/*
 * Bind<> for basic binding
 * BindExicutionOrder<> for order dependent
 * BindInterfaces...<> for multiple interfaces to 1 obj
 * 
 * To<> for basic binding
 * WhenInjectedInto<> for utils mostly
 * WithArguments<> for constructors
 * 
 * From... for monobehaviours
 * 
 * AsSingle() for basic 1-1 binding
 * AsTransient() new obj every inject
 * AsCached() for chaches
 * https://github.com/modesttree/Zenject/blob/master/Documentation/CheatSheet.md
 */