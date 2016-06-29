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
    public class WallHittingSystem : IReactToDataSystem<WallHitEvent>
    {
        private readonly IGroup _targetGroup = new Group(typeof (PlayerComponent), typeof (ViewComponent));
        private readonly IEventSystem _eventSystem;
        private readonly IPoolManager _poolManager;

        public IGroup TargetGroup
        {
            get { return _targetGroup; }
        }

        public WallHittingSystem(IEventSystem eventSystem, IPoolManager poolManager)
        {
            _eventSystem = eventSystem;
            _poolManager = poolManager;
        }

        public IObservable<WallHitEvent> ReactToEntity(IEntity entity)
        {
            return _eventSystem.Receive<WallHitEvent>();
        }

        public void Execute(IEntity entity, WallHitEvent reactionData)
        {
            var wallComponent = reactionData.Wall.GetComponent<WallComponent>();
            wallComponent.Health.Value--;

            var viewComponent = entity.GetComponent<ViewComponent>();
            var animator = viewComponent.View.GetComponent<Animator>();
            animator.SetTrigger("playerChop");

            if (wallComponent.Health.Value <= 0)
            {
                var pool = _poolManager.GetContainingPoolFor(reactionData.Wall);
                pool.RemoveEntity(reactionData.Wall);
            }
        }
    }
}