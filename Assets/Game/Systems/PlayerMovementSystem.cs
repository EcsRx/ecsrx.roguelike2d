using Assets.Game.Components;
using Assets.Game.Configuration;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class PlayerMovementSystem : IReactToGroupSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(MovementComponent), typeof(PlayerComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        private readonly IEventSystem _eventSystem;

        public IObservable<IGroupAccessor> ReactToGroup(IGroupAccessor @group)
        {
            return _eventSystem.Receive<PlayerTurnEvent>().Select(x => @group);
        }

        public PlayerMovementSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public void Execute(IEntity entity)
        {
            var movementComponent = entity.GetComponent<MovementComponent>();

            if (movementComponent.Movement.Value != Vector2.zero)
            { return; }
            
            if (entity.HasComponent<StandardInputComponent>())
            {
                var inputComponent = entity.GetComponent<StandardInputComponent>();
                movementComponent.Movement.Value = inputComponent.PendingMovement;
                inputComponent.PendingMovement = Vector2.zero;
                return;
            }

            if (entity.HasComponent<TouchInputComponent>())
            {
                var inputComponent = entity.GetComponent<TouchInputComponent>();
                movementComponent.Movement.Value = inputComponent.PendingMovement;
                inputComponent.PendingMovement = Vector2.zero;
                return;
            }
        }
    }
}