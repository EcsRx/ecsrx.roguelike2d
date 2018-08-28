using System;
using Assets.Game.Components;
using Assets.Game.Configuration;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class PlayerMovementSystem : IReactToGroupSystem
    {
        public IGroup Group { get; } = new Group(typeof(MovementComponent), typeof(PlayerComponent));

        private readonly IEventSystem _eventSystem;

        public IObservable<IObservableGroup> ReactToGroup(IObservableGroup group)
        { return _eventSystem.Receive<PlayerTurnEvent>().Select(x => group); }

        public PlayerMovementSystem(IEventSystem eventSystem)
        { _eventSystem = eventSystem; }

        public void Process(IEntity entity)
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