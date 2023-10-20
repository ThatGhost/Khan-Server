using Server.Core;
using Server.Services;
using Server.Behaviours;
using Server.EntryPoints;

using UnityEngine;
using Zenject;
using Server.Magic;

public class MonoServerInstaller : MonoInstaller
{
    public Transform g_root;
    public Transform g_spellPoolObject;
    public Transform g_SpawnPoint;

    // prefabs
    [SerializeField] private GameObject playerPrefab;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameServer>().AsSingle().NonLazy();
        Container.Bind<ICoder>().To<Coder>().AsSingle();
        Container.Bind<IMessageQueue>().To<MessageQueue>().AsSingle();
        Container.Bind<IEntryPointRegistry>().To<EntryPointRegistry>().AsSingle();
        Container.BindInstance<Transform>(g_root);
        Container.BindInstance<Transform>(g_spellPoolObject).WithId("spellPoolRoot");
        Container.BindInstance<Transform>(g_SpawnPoint).WithId("spawnPoint");
        SignalBusInstaller.Install(Container);

        registerEntryPoints();
        registerServices();
        registerBehaviours();
        registerFactories();
        registerSignals();
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
        Container.Bind<IClockService>().To<ClockService>().AsTransient();
        Container.Bind<ILoggerService>().To<LoggerService>().AsTransient();
        Container.BindInterfacesAndSelfTo<ClientInitializerService>().AsTransient().NonLazy();
        Container.Bind<IClientDestructorService>().To<ClientDestructorService>().AsTransient();
        Container.Bind<IMessagePublisher>().To<MessagePublisher>().AsSingle();
        Container.Bind<IPlayersController>().To<PlayersController>().AsSingle().NonLazy();
        Container.Bind<IPlayerInputService>().To<PlayerInputService>().AsTransient();
        Container.BindInterfacesAndSelfTo<PlayerPositionService>().AsSingle().NonLazy();
        Container.Bind<ISpellInitializer>().To<SpellInitializer>().AsSingle();
        Container.Bind<IPlayerDeathService>().To<PlayerDeathService>().AsSingle().NonLazy();
    }

    private void registerBehaviours()
    {
        // Container.Bind<TestBehaviour>().FromComponentOn(g_root.gameObject).AsSingle();
        Container.Bind<IDebugStatBehaviour>().To<DebugStatBehaviour>().FromComponentOn(g_root.gameObject).AsSingle();
    }

    private void registerFactories()
    {
        Container.BindFactory<PlayerBehaviour, PlayerBehaviour.Factory>().FromComponentInNewPrefab(playerPrefab);
    }

    private void registerSignals()
    {
        Container.DeclareSignal<OnManaSignal>();
        Container.DeclareSignal<OnHealthSignal>();
        Container.DeclareSignal<OnDeathSignal>();
    }
}

/*
 * Bind<> for basic binding
 * BindExicutionOrder<> for order dependent
 * Con
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
 * AsCached() for singleton for the same contract type
 * https://github.com/modesttree/Zenject/blob/master/Documentation/CheatSheet.md
 */