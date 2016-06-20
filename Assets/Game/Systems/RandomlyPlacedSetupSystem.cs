using System.Linq;
using Assets.EcsRx.Framework.Attributes;
using Assets.Game.Components;
using Assets.Game.Extensions;
using Assets.Game.Groups;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UnityEngine;

namespace Assets.Game.Systems
{
    [Priority(11)]
    public class RandomlyPlacedSetupSystem : ISetupSystem
    {
        private readonly IGroup _targetGroup = new RandomlyPlacedGroup();
        private readonly IGroup _gameBoardGroup = new GameBoardGroup();

        public IGroup TargetGroup { get { return _targetGroup; } }

        public IPoolManager PoolManager { get; private set; }

        public RandomlyPlacedSetupSystem(IPoolManager poolManager)
        {
            PoolManager = poolManager;
        }

        public void Setup(IEntity entity)
        {
            var gameBoardEntity = PoolManager.GetEntitiesFor(_gameBoardGroup).First();
            var gameBoardComponent = gameBoardEntity.GetComponent<GameBoardComponent>();

            var viewComponent = entity.GetComponent<ViewComponent>();
            var randomlyPlacedComponent = entity.GetComponent<RandomlyPlacedComponent>();
            var randomPosition = gameBoardComponent.OpenTiles.TakeRandom();
            randomlyPlacedComponent.RandomPosition = randomPosition;
            viewComponent.View.transform.localPosition = randomPosition;
            gameBoardComponent.OpenTiles.Remove(randomPosition);
        }
    }
}