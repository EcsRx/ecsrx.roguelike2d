using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class WallHittingSystem : IReactToDataSystem<WallHitEvent>
    {
        private readonly IGroup _targetGroup = new Group(typeof(PlayerComponent), typeof(ViewComponent));
        private readonly IEventSystem _eventSystem;

        public IGroup TargetGroup { get { return _targetGroup; } }

        public WallHittingSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
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
        }
    }
}