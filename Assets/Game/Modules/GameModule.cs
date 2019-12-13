using EcsRx.Infrastructure.Dependencies;
using EcsRx.Infrastructure.Extensions;

using Game.Configuration;

namespace Game.Installers
{
    public class GameModule : IDependencyModule
    {
        public void Setup(IDependencyContainer container)
        {
            container.Bind<GameConfiguration>(new BindingConfiguration{AsSingleton = true});
        }
    }
}