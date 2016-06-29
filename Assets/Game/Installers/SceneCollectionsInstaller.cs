using Assets.Game.SceneCollections;
using Zenject;

namespace Assets.Game.Installers
{
    public class SceneCollectionsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SetupTiles();
            SetupAudio();
        }

        private void SetupTiles()
        {
            Container.Bind<FloorTiles>().AsSingle();
            Container.Bind<OuterWallTiles>().AsSingle();
            Container.Bind<WallTiles>().AsSingle();
            Container.Bind<FoodTiles>().AsSingle();
            Container.Bind<EnemyTiles>().AsSingle();
            Container.Bind<ExitTiles>().AsSingle();
        }

        private void SetupAudio()
        {
            Container.Bind<EnemyAttackSounds>().AsSingle();
            Container.Bind<PlayerAttackSounds>().AsSingle();
            Container.Bind<WalkingSounds>().AsSingle();
            Container.Bind<DeathSounds>().AsSingle();
            Container.Bind<DrinkSounds>().AsSingle();
            Container.Bind<FoodSounds>().AsSingle();
        }
    }
}