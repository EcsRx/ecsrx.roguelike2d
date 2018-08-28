using System.Linq;
using Assets.Game.Components;
using Assets.Game.Extensions;
using Assets.Game.Groups;
using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
namespace Assets.Game.Systems
{
    [Priority(11)]
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