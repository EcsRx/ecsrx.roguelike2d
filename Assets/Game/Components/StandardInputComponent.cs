using EcsRx.Components;
using UnityEngine;

namespace Game.Components
{
    public class StandardInputComponent : IComponent
    {
        public Vector2 PendingMovement { get; set; }
    }
}