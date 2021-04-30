using System.Linq;
using SystemsRx.Attributes;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Plugins.ReactiveSystems.Systems;
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
        public IEntityDatabase EntityDatabase { get; }
        
        public IGroup Group { get; } = new RandomlyPlacedGroup();
        
        public RandomlyPlacedSetupSystem(IEntityDatabase entityDatabase)
        { EntityDatabase = entityDatabase; }

        public void Setup(IEntity entity)
        {
            var gameBoardEntity = EntityDatabase.GetEntitiesFor(_gameBoardGroup).First();
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