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
    public class EnemyAttackedSys : EventReactionSystem<EnemyHitEvent>
    {
        private readonly IPoolManager _poolManager;

        public EnemyAttackedSys(IEventSystem eventSystem, IPoolManager poolManager) : base(eventSystem)
        { _poolManager = poolManager; }

        public override void EventTriggered(EnemyHitEvent eventData)
        {
            var enemyComponent = eventData.Enemy.GetComponent<EnemyComponent>();
            enemyComponent.Health.Value--;

            var viewComponent = eventData.Player.GetComponent<ViewComponent>();
            var animator = viewComponent.View.GetComponent<Animator>();
            animator.SetTrigger("playerChop");

            if (enemyComponent.Health.Value <= 0)
            {
                var pool = _poolManager.GetContainingPoolFor(eventData.Enemy);
                pool.RemoveEntity(eventData.Enemy);
            }
        }
    }
}