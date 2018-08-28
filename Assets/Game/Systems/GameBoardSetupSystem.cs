using Assets.Game.Components;
using Assets.Game.Groups;
using EcsRx.Attributes;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using UnityEngine;

namespace Assets.Game.Systems
{
    [Priority(10)]
    public class GameBoardSetupSystem : ISetupSystem
    {
        public IGroup Group { get; } = new GameBoardGroup();

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