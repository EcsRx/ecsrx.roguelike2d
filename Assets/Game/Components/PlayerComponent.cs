using EcsRx.Components;
using UniRx;

namespace Assets.Game.Components
{
    public class PlayerComponent : IComponent
    {
        public ReactiveProperty<int> Food { get; set; }

        public PlayerComponent()
        {
            Food = new IntReactiveProperty();
        }
    }
}