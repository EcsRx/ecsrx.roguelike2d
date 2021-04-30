using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using Game.Configuration;

namespace Game.Installers
{
    public class GameModule : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<GameConfiguration>(x => x.AsSingleton());
        }
    }
}