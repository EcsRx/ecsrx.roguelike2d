using System;
using Assets.Game.Components;
using EcsRx.Groups;
using EcsRx.Views.Components;

namespace Assets.Game.Groups
{
    public class GameBoardGroup : IGroup
    {
        public Type[] RequiredComponents { get; } = {typeof (ViewComponent), typeof (GameBoardComponent)};
        public Type[] ExcludedComponents { get; } = new Type[0];
    }
}