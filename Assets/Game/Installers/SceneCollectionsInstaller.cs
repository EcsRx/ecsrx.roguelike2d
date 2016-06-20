using Assets.Game.SceneCollections;
using Zenject;

namespace Assets.Game.Installers
{
    public class SceneCollectionsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<FloorTiles>().AsSingle();
            Container.Bind<OuterWallTiles>().AsSingle();
            Container.Bind<WallTiles>().AsSingle();
            Container.Bind<FoodTiles>().AsSingle();
            Container.Bind<EnemyTiles>().AsSingle();
            Container.Bind<ExitTiles>().AsSingle();
        }
    }
}