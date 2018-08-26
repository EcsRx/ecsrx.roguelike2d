using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Collections;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Systems.Custom;
using EcsRx.Views.Components;
using UnityEngine;

namespace Assets.Game.Systems
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

            var viewComponent = eventData.Player.GetComponent<ViewComponent>();
            var gameObject = viewComponent.View as GameObject;
            var animator = gameObject.GetComponent<Animator>();
            animator.SetTrigger("playerChop");

            if (enemyComponent.Health.Value <= 0)
            {
                var collection = _entityCollectionManager.GetCollectionFor(eventData.Enemy);
                collection.RemoveEntity(eventData.Enemy.Id);
            }
        }
    }
}