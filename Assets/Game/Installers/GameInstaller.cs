using Assets.Game.Configuration;
using Zenject;

namespace Assets.Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameConfiguration>().AsSingle();
        }
    }
}