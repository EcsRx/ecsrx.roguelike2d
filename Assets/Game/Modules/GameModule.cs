using EcsRx.Infrastructure.Dependencies;
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