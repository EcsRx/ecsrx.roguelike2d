using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Pools;
using EcsRx.Systems.Custom;

namespace Assets.Game.Systems
{
    public class FoodPickupSystem : EventReactionSystem<FoodPickupEvent>
    {
        private IPoolManager _poolManager;

        public FoodPickupSystem(IEventSystem eventSystem, IPoolManager poolManager) : base(eventSystem)
        {
            _poolManager = poolManager;
        }

        public override void EventTriggered(FoodPickupEvent eventData)
        {
            var playerComponent = eventData.Player.GetComponent<PlayerComponent>();
            var foodComponent = eventData.Food.GetComponent<FoodComponent>();

            playerComponent.Food.Value += foodComponent.FoodAmount;
            _poolManager.RemoveEntity(eventData.Food);
        }
    }
}