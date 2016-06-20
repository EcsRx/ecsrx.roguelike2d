using EcsRx.Components;
using UnityEngine;

namespace Assets.Game.Components
{
    public class TouchInputComponent : IComponent
    {
        public Vector2 TouchOrigin { get; set; }
    }
}