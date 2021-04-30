using SystemsRx.Systems.Conventional;
using EcsRx.Collections.Database;
using EcsRx.Extensions;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class EnemyAttackedSystem : IReactToEventSystem<EnemyHitEvent>
    {
        private readonly IEntityDatabase _entityDatabase;

        public EnemyAttackedSystem(IEntityDatabase entityDatabase)
        { _entityDatabase = entityDatabase; }

        public void Process(EnemyHitEvent eventData)
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