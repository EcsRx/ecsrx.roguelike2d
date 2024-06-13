using SystemsRx.Attributes;
using SystemsRx.Types;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using Game.Components;
using Game.Groups;
using UnityEngine;

namespace Game.Systems
{
    [Priority(PriorityTypes.High)]
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