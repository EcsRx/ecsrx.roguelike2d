using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using Game.SceneCollections;

namespace Game.Installers
{
    public class SceneCollectionsModule : IDependencyModule
    {
        private void SetupTiles(IDependencyRegistry registry)
        {
            registry.Bind<FloorTiles>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<OuterWallTiles>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<WallTiles>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<FoodTiles>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<EnemyTiles>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<ExitTiles>(new BindingConfiguration{AsSingleton = true});
        }

        private void SetupAudio(IDependencyRegistry registry)
        {
            registry.Bind<EnemyAttackSounds>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<PlayerAttackSounds>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<WalkingSounds>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<DeathSounds>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<DrinkSounds>(new BindingConfiguration{AsSingleton = true});
            registry.Bind<FoodSounds>(new BindingConfiguration{AsSingleton = true});
        }

        public void Setup(IDependencyRegistry container)
        {
            SetupTiles(container);
            SetupAudio(container);
        }
    }
}