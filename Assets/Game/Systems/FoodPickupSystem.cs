using EcsRx.Collections;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Systems.Custom;
using Game.Components;
using Game.Events;

namespace Game.Systems
{
    public class FoodPickupSystem : EventReactionSystem<FoodPickupEvent>
    {
        private readonly IEntityCollectionManager _entityCollectionManager;

        public FoodPickupSystem(IEventSystem eventSystem, IEntityCollectionManager entityCollectionManager) : base(eventSystem)
        { _entityCollectionManager = entityCollectionManager; }

        public override void EventTriggered(FoodPickupEvent eventData)
        {
            var playerComponent = eventData.Player.GetComponent<PlayerComponent>();
            var foodComponent = eventData.Food.GetComponent<FoodComponent>();

            playerComponent.Food.Value += foodComponent.FoodAmount;
            _entityCollectionManager.RemoveEntity(eventData.Food);
        }
    }
}