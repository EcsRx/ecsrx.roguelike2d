using EcsRx.Components;
using UniRx;
using UnityEngine;

namespace Assets.Game.Components
{
    public class RandomlyPlacedComponent : IComponent
    {
        public Vector3 RandomPosition { get; set; }
    }
}