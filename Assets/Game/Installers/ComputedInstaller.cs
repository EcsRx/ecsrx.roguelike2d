using Assets.Game.Components;
using EcsRx.Unity.Extensions;
using Game.Computeds;
using Zenject;

namespace Assets.Game.Installers
{
    public class ComputedInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IComputedPlayerPosition>().FromMethod(x =>
            {
                var observableGroup = x.Container.ResolveObservableGroup(typeof(PlayerComponent));
                return new ComputedPlayerPosition(observableGroup);
            });
        }
    }
}