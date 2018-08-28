using System;
using EcsRx.Components;
using UniRx;

namespace Game.Components
{
    public class WallComponent : IComponent, IDisposable
    {
         public ReactiveProperty<int> Health { get; set; }

        public WallComponent()
        {
            Health = new IntReactiveProperty();
        }

        public void Dispose()
        {
            Health.Dispose();
        }
    }
}