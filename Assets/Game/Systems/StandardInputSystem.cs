using Assets.Game.Components;
using EcsRx.Entities;
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

        public IObservable<IGroupAccessor> ReactToGroup(IGroupAccessor @group)
        {
            return Observable.EveryUpdate().Select(x => @group);
        }

        public void Execute(IEntity entity)
        {
            var movementComponent = entity.GetComponent<MovementComponent>();
            if(movementComponent.Movement.Value != Vector2.zero) { return; }

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