using System;
using EcsRx.Components;
using UniRx;

namespace Game.Components
{
    public class PlayerComponent : IComponent, IDisposable
    {
        public ReactiveProperty<int> Food { get; set; }

        public PlayerComponent()
        {
            Food = new IntReactiveProperty();
        }

        public void Dispose()
        {
            Food.Dispose();
        }
    }
}