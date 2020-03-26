using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Plugins.ReactiveSystems.Custom;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;

namespace Game.Systems
{
    [Priority(-10)]
    public class FoodPickupSystem : EventReactionSystem<FoodPickupEvent>
    {
        private readonly IEntityDatabase _entityDatabase;

        public FoodPickupSystem(IEventSystem eventSystem, IEntityDatabase entityDatabase) : base(eventSystem)
        { _entityDatabase = entityDatabase; }

        public override void EventTriggered(FoodPickupEvent eventData)
        {
            var playerComponent = eventData.Player.GetComponent<PlayerComponent>();
            var foodComponent = eventData.Food.GetComponent<FoodComponent>();

            playerComponent.Food.Value += foodComponent.FoodAmount;

            this.AfterUpdateDo(x => { _entityDatabase.RemoveEntity(eventData.Food); });
        }
    }
}