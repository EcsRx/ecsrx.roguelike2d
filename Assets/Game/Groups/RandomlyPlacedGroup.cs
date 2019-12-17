using System;
using EcsRx.Groups;
using EcsRx.Plugins.Views.Components;
using Game.Components;

namespace Game.Groups
{
    public class RandomlyPlacedGroup : IGroup
    {
        public Type[] RequiredComponents { get; } = { typeof(ViewComponent), typeof(RandomlyPlacedComponent) };
        public Type[] ExcludedComponents { get; } = new Type[0];
    }
}