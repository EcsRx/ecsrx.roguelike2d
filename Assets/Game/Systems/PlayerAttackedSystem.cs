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
    public class PlayerAttackedSystem : IReactToDataSystem<PlayerHitEvent>
    {
        private readonly IGroup _targetGroup = new Group(typeof(EnemyComponent), typeof(ViewComponent));
        private readonly IEventSystem _eventSystem;

        public IGroup TargetGroup { get { return _targetGroup; } }

        public PlayerAttackedSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public IObservable<PlayerHitEvent> ReactToEntity(IEntity entity)
        {
            return _eventSystem.Receive<PlayerHitEvent>().Where(x => x.Enemy == entity);
        }

        public void Execute(IEntity entity, PlayerHitEvent reactionData)
        {
            var playerComponent = reactionData.Player.GetComponent<PlayerComponent>();
            playerComponent.Food.Value -= 10;

            var viewComponent = entity.GetComponent<ViewComponent>();
            var animator = viewComponent.View.GetComponent<Animator>();
            animator.SetTrigger("enemyAttack");
        }
    }
}