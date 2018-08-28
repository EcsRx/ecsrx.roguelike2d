using Game.Configuration;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameConfiguration>().AsSingle();
        }
    }
}