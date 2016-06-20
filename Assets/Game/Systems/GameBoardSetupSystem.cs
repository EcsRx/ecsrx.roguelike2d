using Assets.EcsRx.Framework.Attributes;
using Assets.Game.Components;
using Assets.Game.Groups;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UnityEngine;

namespace Assets.Game.Systems
{
    [Priority(10)]
    public class GameBoardSetupSystem : ISetupSystem
    {
        private readonly IGroup _targetGroup = new GameBoardGroup();
        public IGroup TargetGroup
        {
            get { return _targetGroup; }
        }

        public void Setup(IEntity entity)
        {
            var gameBoardComponent = entity.GetComponent<GameBoardComponent>();

            for (var x = 1; x < gameBoardComponent.Width - 1; x++)
            {
                for (var y = 1; y < gameBoardComponent.Height - 1; y++)
                {
                    gameBoardComponent.OpenTiles.Add(new Vector3(x, y, 0f));
                }
            }
        }
    }
}