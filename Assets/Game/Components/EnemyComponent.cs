using System;
using EcsRx.Components;
using Game.Enums;
using UniRx;

namespace Game.Components
{
    public class EnemyComponent : IComponent, IDisposable
    {
        public ReactiveProperty<int> Health { get; set; }
        public EnemyTypes EnemyType { get; set; }
        public int EnemyPower { get; set; }
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