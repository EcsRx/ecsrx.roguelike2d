using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using Game.Configuration;

namespace Game.Installers
{
    public class GameModule : IDependencyModule
    {
        public void Setup(IDependencyRegistry registry)
        {
            registry.Bind<GameConfiguration>(x => x.AsSingleton());
        }
    }
}