using EcsRx.Components;
using UnityEngine;

namespace Assets.Game.Components
{
    public class TouchInputComponent : IComponent
    {
        public Vector2 TouchOrigin { get; set; }
        public Vector2 PendingMovement { get; set; }
    }
}