using Networking.Core;
using Networking.Services;
using Networking.Behaviours;
using Networking.EntryPoints;

using UnityEngine;
using Zenject;

public class MonoServerInstaller : MonoInstaller
{
    // root
    public Transform g_root;

    // prefabs
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spell_Fire_FlameTower;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameServer>().AsSingle().NonLazy();
        Container.Bind<ICoder>().To<Coder>().AsSingle();
        Container.Bind<IMessageQueue>().To<MessageQueue>().AsSingle();
        Container.Bind<IEntryPointRegistry>().To<EntryPointRegistry>().AsSingle();
        Container.BindInstance<Transform>(g_root);

        registerEntryPoints();
        registerServices();
        registerBehaviours();
        registerFactories();
    }

    private void registerEntryPoints()
    {
        //Container.Bind<ITestEntryPoint>().To<TestEntryPoint>().AsSingle();
        Container.Bind<IPlayerEntryPoint>().To<PlayerEntryPoint>().AsSingle();
    }

    private void registerServices()
    {
        //Container.Bind<IFooService>().To<FooService>().AsSingle();
        Container.Bind<IMonoHelper>().To<MonoHelpers>().FromComponentInHierarchy().AsTransient();
        Container.Bind<ILoggerService>().To<LoggerService>().AsSingle();

        Container.BindInterfacesAndSelfTo<ClientInitializerService>().AsSingle().NonLazy();
        Container.Bind<IMessagePublisher>().To<MessagePublisher>().AsSingle();

        Container.Bind<PlayersController>().AsSingle().NonLazy();
        Container.Bind<IPlayerInputService>().To<PlayerInputService>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerPositionService>().AsSingle().NonLazy();

        Container.Bind<ISpellUtil_BasicTimer>().To<SpellUtil_BasicTimer>().AsTransient();
    }

    private void registerBehaviours()
    {
        Container.Bind<TestBehaviour>().FromComponentOn(g_root.gameObject).AsSingle();
    }

    private void registerFactories()
    {
        Container.BindFactory<PlayerBehaviour, PlayerBehaviour.Factory>().FromComponentInNewPrefab(playerPrefab);

        // spells
        Container.BindFactory<Spell_Fire_FlameTower, Spell_Fire_FlameTower.Factory>().FromComponentInNewPrefab(spell_Fire_FlameTower);
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