using EcsRx.Components;
using UniRx;

namespace Assets.Game.Components
{
    public class WallComponent : IComponent
    {
         public ReactiveProperty<int> Health { get; set; }

        public WallComponent()
        {
            Health = new IntReactiveProperty();
        }
    }
}