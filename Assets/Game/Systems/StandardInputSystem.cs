using System;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using Game.Components;
using UniRx;
using UnityEngine;

namespace Game.Systems
{
    public class StandardInputSystem : IReactToGroupSystem
    {
        public IGroup Group { get; } = new Group(typeof(MovementComponent), typeof(StandardInputComponent));

        public IObservable<IObservableGroup> ReactToGroup(IObservableGroup group)
        {
            return Observable.EveryUpdate().Select(x => group);
        }

        public void Process(IEntity entity)
        {
            var movementComponent = entity.GetComponent<MovementComponent>();
            if(movementComponent.Movement.Value != Vector2.zero) { return; }

            var horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            var vertical = (int)(Input.GetAxisRaw("Vertical"));
            
            if (horizontal != 0)
            {
                vertical = 0;
            }

            if (horizontal != 0 || vertical != 0)
            {
                var inputComponent = entity.GetComponent<StandardInputComponent>();
                inputComponent.PendingMovement = new Vector2(horizontal, vertical);
            }
        }
    }
}