using Networking.Core;
using Networking.Services;
using Networking.Behaviours;
using Networking.EntryPoints;

using UnityEngine;
using Zenject;
using Server.Magic;

public class MonoServerInstaller : MonoInstaller
{
    // root
    public Transform g_root;
    public Transform g_spellPoolObject;

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
        Container.BindInstance<Transform>(g_spellPoolObject).WithId("spellPoolRoot");

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
        Container.Bind<IClockService>().To<ClockService>().AsTransient();
        Container.Bind<ILoggerService>().To<LoggerService>().AsTransient();
        Container.BindInterfacesAndSelfTo<ClientInitializerService>().AsTransient().NonLazy();
        Container.Bind<IClientDestructorService>().To<ClientDestructorService>().AsTransient();
        Container.Bind<IMessagePublisher>().To<MessagePublisher>().AsSingle();
        Container.Bind<IPlayersController>().To<PlayersController>().AsSingle().NonLazy();
        Container.Bind<IPlayerInputService>().To<PlayerInputService>().AsTransient();
        Container.BindInterfacesAndSelfTo<PlayerPositionService>().AsSingle().NonLazy();
        Container.Bind<ISpellInitializer>().To<SpellInitializer>().AsSingle();
        Container.Bind<IPlayersVariableService>().To<PlayersVariableService>().AsTransient();
    }

    private void registerBehaviours()
    {
        Container.Bind<TestBehaviour>().FromComponentOn(g_root.gameObject).AsSingle();
    }

    private void registerFactories()
    {
        Container.BindFactory<PlayerBehaviour, PlayerBehaviour.Factory>().FromComponentInNewPrefab(playerPrefab);
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