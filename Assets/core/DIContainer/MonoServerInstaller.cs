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
        Container.Bind<ICoder>().To<Coder>().AsSingle();
        Container.Bind<IMessageQueue>().To<MessageQueue>().AsSingle();
        Container.Bind<IGameServer>().To<GameServer>().AsSingle();
        Container.Bind<IEntryPointRegistry>().To<EntryPointRegistry>().AsSingle();

        // EntryPoints
        Container.Bind<ITestEntryPoint>().To<TestEntryPoint>().AsSingle();

        // Services
        Container.Bind<IFooService>().To<FooService>().AsSingle();

        // Behaviours
        Container.Bind<ITestBehaviour>().To<TestBehaviour>().FromComponentOn(server).AsSingle();
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