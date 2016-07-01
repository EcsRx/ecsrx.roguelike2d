using System;
using Assets.Game.Enums;
using EcsRx.Components;
using UniRx;

namespace Assets.Game.Components
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