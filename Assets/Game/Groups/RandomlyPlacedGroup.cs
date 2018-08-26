using System;
using Assets.Game.Components;
using EcsRx.Groups;
using EcsRx.Views.Components;

namespace Assets.Game.Groups
{
    public class RandomlyPlacedGroup : IGroup
    {
        public Type[] RequiredComponents { get; } = { typeof(ViewComponent), typeof(RandomlyPlacedComponent) };
        public Type[] ExcludedComponents { get; } = new Type[0];
    }
}