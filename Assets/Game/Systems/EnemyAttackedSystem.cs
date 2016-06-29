using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class EnemyAttackedSystem : IReactToDataSystem<EnemyHitEvent>
    {
        private readonly IGroup _targetGroup = new Group(typeof(PlayerComponent), typeof(ViewComponent));
        private readonly IEventSystem _eventSystem;
        private readonly IPoolManager _poolManager;

        public IGroup TargetGroup { get { return _targetGroup; } }

        public EnemyAttackedSystem(IEventSystem eventSystem, IPoolManager poolManager)
        {
            _eventSystem = eventSystem;
            _poolManager = poolManager;
        }

        public IObservable<EnemyHitEvent> ReactToEntity(IEntity entity)
        {
            return _eventSystem.Receive<EnemyHitEvent>();
        }

        public void Execute(IEntity entity, EnemyHitEvent reactionData)
        {
            var enemyComponent = reactionData.Enemy.GetComponent<EnemyComponent>();
            enemyComponent.Health.Value--;

            var viewComponent = entity.GetComponent<ViewComponent>();
            var animator = viewComponent.View.GetComponent<Animator>();
            animator.SetTrigger("playerChop");

            if (enemyComponent.Health.Value <= 0)
            {
                var pool = _poolManager.GetContainingPoolFor(reactionData.Enemy);
                pool.RemoveEntity(reactionData.Enemy);
            }
        }
    }
}