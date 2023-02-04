using Networking.Core;
using Networking.Services;
using Networking.Behaviours;

using UnityEngine;
using Zenject;

public class MonoServerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IFooService>().To<FooService>().AsSingle();
        Container.Bind<ICoder>().To<Coder>().AsSingle();
        Container.Bind<IMessageQueue>().To<MessageQueue>().AsSingle();
        Container.Bind<IGameServer>().To<GameServer>().AsSingle();
        Container.Bind<ITestBehaviour>().To<TestBehaviour>().AsSingle();
    }
}

/*
 * Bind<> for basic binding
 * BindExicutionOrder<> for order dependent
 * BindInterfacec...<> for multiple interfaces to 1 obj
 * 
 * To<> for basic binding
 * WhenInjectedInto<> for utils mostly
 * WithArguments<> for constructors
 * 
 * AsSingle() for basic 1-1 binding
 * AsTransient() new obj every inject
 * AsCached() for chaches
 * https://github.com/modesttree/Zenject/blob/master/Documentation/CheatSheet.md
 */