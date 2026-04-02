using BugColony.Bugs;
using BugColony.Environment;
using BugColony.Spawning;
using BugColony.Strategies;
using Zenject;

namespace BugColony.Infrastructure
{
    public class BugSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<BugKilledSignal>();

            Container.Bind<ColonyController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GroundController>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BugSpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<FoodSpawner>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IBugStrategy>().To<BugStrategyWorker>().AsSingle();
            Container.Bind<IBugStrategy>().To<BugStrategyPredator>().AsSingle();
            Container.Bind<BugStrategyController>().AsSingle().NonLazy();
        }
    }
}