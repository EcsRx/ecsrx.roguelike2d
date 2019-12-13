using EcsRx.Attributes;
using EcsRx.Collections;
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
        private readonly IEntityCollectionManager _entityCollectionManager;

        public FoodPickupSystem(IEventSystem eventSystem, IEntityCollectionManager entityCollectionManager) : base(eventSystem)
        { _entityCollectionManager = entityCollectionManager; }

        public override void EventTriggered(FoodPickupEvent eventData)
        {
            var playerComponent = eventData.Player.GetComponent<PlayerComponent>();
            var foodComponent = eventData.Food.GetComponent<FoodComponent>();

            playerComponent.Food.Value += foodComponent.FoodAmount;

            this.AfterUpdateDo(x => { _entityCollectionManager.RemoveEntity(eventData.Food); });
        }
    }
}