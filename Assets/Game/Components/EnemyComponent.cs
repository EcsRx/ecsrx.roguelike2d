using System;
using EcsRx.Components;
using UniRx;

namespace Assets.Game.Components
{
    public class EnemyComponent : IComponent, IDisposable
    {
        public ReactiveProperty<int> Health { get; set; }
        public bool IsSkippingNextTurn { get; set; }

        public EnemyComponent()
        {
            Health = new IntReactiveProperty();
        }

        public void Dispose()
        {
            Health.Dispose();
        }
    }
}