using EcsRx.Infrastructure.Dependencies;
using Game.SceneCollections;
using Zenject;

namespace Game.Installers
{
    public class SceneCollectionsModule : IDependencyModule
    {
        private void SetupTiles(IDependencyContainer container)
        {
            container.Bind<FloorTiles>(new BindingConfiguration{AsSingleton = true});
            container.Bind<OuterWallTiles>(new BindingConfiguration{AsSingleton = true});
            container.Bind<WallTiles>(new BindingConfiguration{AsSingleton = true});
            container.Bind<FoodTiles>(new BindingConfiguration{AsSingleton = true});
            container.Bind<EnemyTiles>(new BindingConfiguration{AsSingleton = true});
            container.Bind<ExitTiles>(new BindingConfiguration{AsSingleton = true});
        }

        private void SetupAudio(IDependencyContainer container)
        {
            container.Bind<EnemyAttackSounds>(new BindingConfiguration{AsSingleton = true});
            container.Bind<PlayerAttackSounds>(new BindingConfiguration{AsSingleton = true});
            container.Bind<WalkingSounds>(new BindingConfiguration{AsSingleton = true});
            container.Bind<DeathSounds>(new BindingConfiguration{AsSingleton = true});
            container.Bind<DrinkSounds>(new BindingConfiguration{AsSingleton = true});
            container.Bind<FoodSounds>(new BindingConfiguration{AsSingleton = true});
        }

        public void Setup(IDependencyContainer container)
        {
            SetupTiles(container);
            SetupAudio(container);
        }
    }
}