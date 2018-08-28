using EcsRx.Collections;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Systems.Custom;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class EnemyAttackedSystem : EventReactionSystem<EnemyHitEvent>
    {
        private readonly IEntityCollectionManager _entityCollectionManager;

        public EnemyAttackedSystem(IEventSystem eventSystem, IEntityCollectionManager entityCollectionManager) : base(eventSystem)
        { _entityCollectionManager = entityCollectionManager; }

        public override void EventTriggered(EnemyHitEvent eventData)
        {
            var enemyComponent = eventData.Enemy.GetComponent<EnemyComponent>();
            enemyComponent.Health.Value--;

            var animator = eventData.Player.GetUnityComponent<Animator>();
            animator.SetTrigger("playerChop");

            if (enemyComponent.Health.Value <= 0)
            { _entityCollectionManager.RemoveEntity(eventData.Enemy); }
        }
    }
}