using System;
using EcsRx.Groups;
using EcsRx.Views.Components;
using Game.Components;

namespace Game.Groups
{
    public class GameBoardGroup : IGroup
    {
        public Type[] RequiredComponents { get; } = {typeof (ViewComponent), typeof (GameBoardComponent)};
        public Type[] ExcludedComponents { get; } = new Type[0];
    }
}