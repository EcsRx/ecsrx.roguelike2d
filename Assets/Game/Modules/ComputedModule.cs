using SystemsRx.Infrastructure.Dependencies;
using SystemsRx.Infrastructure.Extensions;
using EcsRx.Infrastructure.Extensions;
using Game.Components;
using Game.Computeds;

namespace Game.Installers
{
    public class ComputedModule : IDependencyModule
    {
        public void Setup(IDependencyRegistry registry)
        {
            registry.Bind<IComputedPlayerPosition>(c =>
            {
                c.ToMethod(x =>
                {
                    var observableGroup = x.ResolveObservableGroup(typeof(PlayerComponent));
                    return new ComputedPlayerPosition(observableGroup);
                });
            });
        }
    }
}