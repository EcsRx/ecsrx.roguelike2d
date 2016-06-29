using System;
using System.Collections.Generic;
using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class StandardInputSystem : IReactToGroupSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(MovementComponent), typeof(StandardInputComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        public IObservable<GroupAccessor> ReactToGroup(GroupAccessor @group)
        {
            return Observable.EveryUpdate().Select(x => @group);
        }

        public void Execute(IEntity entity)
        {
            var horizontal = 0;
            var vertical = 0;

            horizontal = (int)(Input.GetAxisRaw("Horizontal"));
            vertical = (int)(Input.GetAxisRaw("Vertical"));
            
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