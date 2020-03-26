using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Plugins.ReactiveSystems.Custom;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class EnemyAttackedSystem : EventReactionSystem<EnemyHitEvent>
    {
        private readonly IEntityDatabase _entityDatabase;

        public EnemyAttackedSystem(IEventSystem eventSystem, IEntityDatabase entityDatabase) : base(eventSystem)
        { _entityDatabase = entityDatabase; }

        public override void EventTriggered(EnemyHitEvent eventData)
        {
            var enemyComponent = eventData.Enemy.GetComponent<EnemyComponent>();
            enemyComponent.Health.Value--;

            var animator = eventData.Player.GetUnityComponent<Animator>();
            animator.SetTrigger("playerChop");

            if (enemyComponent.Health.Value <= 0)
            { _entityDatabase.RemoveEntity(eventData.Enemy); }
        }
    }
}