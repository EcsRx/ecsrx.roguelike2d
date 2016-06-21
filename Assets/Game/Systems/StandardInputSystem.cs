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
        private IGroup _targetGroup = new Group(typeof(MovementComponent), typeof(StandardInputComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        public IObservable<GroupAccessor> ReactToGroup(GroupAccessor @group)
        {
            return Observable.EveryUpdate().Select(x => @group);
        }

        public void Execute(IEntity entity)
        {
            var movementComponent = entity.GetComponent<MovementComponent>();
            
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
                Debug.Log(string.Format("Variables: x:{0}, y:{1}", horizontal, vertical));
                var movement = new Vector2(horizontal, vertical);
                Debug.Log("Movement Var: " + movement);
                Debug.Log("Reactive Var Before: " + movementComponent.Movement.Value);
                movementComponent.Movement.Value = movement;
                Debug.Log("Reactive Var After: " + movementComponent.Movement.Value);
            }
        }
    }
}