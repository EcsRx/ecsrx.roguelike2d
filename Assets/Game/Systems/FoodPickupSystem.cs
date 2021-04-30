using SystemsRx.Attributes;
using SystemsRx.Systems.Conventional;
using SystemsRx.Types;
using EcsRx.Collections.Database;
using EcsRx.Extensions;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;

namespace Game.Systems
{
    [Priority(PriorityTypes.Low)]
    public class FoodPickupSystem : IReactToEventSystem<FoodPickupEvent>
    {
        private readonly IEntityDatabase _entityDatabase;

        public FoodPickupSystem(IEntityDatabase entityDatabase)
        { _entityDatabase = entityDatabase; }

        public void Process(FoodPickupEvent eventData)
        {
            var playerComponent = eventData.Player.GetComponent<PlayerComponent>();
            var foodComponent = eventData.Food.GetComponent<FoodComponent>();

            playerComponent.Food.Value += foodComponent.FoodAmount;

            this.AfterUpdateDo(x => { _entityDatabase.RemoveEntity(eventData.Food); });
        }
    }
}