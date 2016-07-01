using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Pools;
using EcsRx.Systems.Custom;
using EcsRx.Unity.Components;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class WallHitSystem : EventReactionSystem<WallHitEvent>
    {
        private readonly IPoolManager _poolManager;

        public WallHitSystem(IEventSystem eventSystem, IPoolManager poolManager) : base(eventSystem)
        { _poolManager = poolManager; }

        public override void EventTriggered(WallHitEvent eventData)
        {
            var wallComponent = eventData.Wall.GetComponent<WallComponent>();
            wallComponent.Health.Value--;

            var viewComponent = eventData.Player.GetComponent<ViewComponent>();
            var animator = viewComponent.View.GetComponent<Animator>();
            animator.SetTrigger("playerChop");

            if (wallComponent.Health.Value <= 0)
            {
                var pool = _poolManager.GetContainingPoolFor(eventData.Wall);
                pool.RemoveEntity(eventData.Wall);
            }
        }
    }
}