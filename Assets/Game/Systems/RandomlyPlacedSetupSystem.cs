using System.Linq;
using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Extensions;
using Game.Groups;

namespace Game.Systems
{
    [Priority(1)]
    public class RandomlyPlacedSetupSystem : ISetupSystem
    {
        private readonly IGroup _gameBoardGroup = new GameBoardGroup();
        public IEntityCollectionManager EntityCollectionManager { get; }
        
        public IGroup Group { get; } = new RandomlyPlacedGroup();
        
        public RandomlyPlacedSetupSystem(IEntityCollectionManager entityCollectionManager)
        { EntityCollectionManager = entityCollectionManager; }

        public void Setup(IEntity entity)
        {
            var gameBoardEntity = EntityCollectionManager.GetEntitiesFor(_gameBoardGroup).First();
            var gameBoardComponent = gameBoardEntity.GetComponent<GameBoardComponent>();

            var viewComponent = entity.GetGameObject();
            var randomlyPlacedComponent = entity.GetComponent<RandomlyPlacedComponent>();
            var randomPosition = gameBoardComponent.OpenTiles.TakeRandom();
            randomlyPlacedComponent.RandomPosition = randomPosition;
            viewComponent.transform.localPosition = randomPosition;
            gameBoardComponent.OpenTiles.Remove(randomPosition);
        }
    }
}