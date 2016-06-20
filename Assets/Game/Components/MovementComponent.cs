using EcsRx.Components;
using UniRx;
using UnityEngine;

namespace Assets.Game.Components
{
    public class MovementComponent : IComponent
    {
        public ReactiveProperty<Vector2> Movement { get; set; }

        public MovementComponent()
        {
            Movement = new Vector2ReactiveProperty();
        }
    }
}