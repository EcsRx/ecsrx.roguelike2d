using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class TouchInputSystem : IReactToGroupSystem
    {
        private IEventSystem _eventSystem;
        private IGroup _targetGroup = new Group(typeof(MovementComponent), typeof(TouchInputComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        public IObservable<GroupAccessor> ReactToGroup(GroupAccessor @group)
        {
            return _eventSystem.Receive<PlayerTurnEvent>().Select(x => @group);
        }

        public TouchInputSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public void Execute(IEntity entity)
        {
            var movementComponent = entity.GetComponent<MovementComponent>();
            var touchComponent = entity.GetComponent<TouchInputComponent>();

            //If it's not the player's turn, exit the function.
            //if (!GameManager.instance.playersTurn) return;

            var horizontal = 0;
            var vertical = 0;
            
            //Check if Input has registered more than zero touches
            if (Input.touchCount > 0)
            {
                //Store the first touch detected.
                Touch myTouch = Input.touches[0];
				
                //Check if the phase of that touch equals Began
                if (myTouch.phase == TouchPhase.Began)
                {
                    //If so, set touchOrigin to the position of that touch
                    touchComponent.TouchOrigin = myTouch.position;
                }
				
                //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
                else if (myTouch.phase == TouchPhase.Ended && touchComponent.TouchOrigin.x >= 0)
                {
                    //Set touchEnd to equal the position of this touch
                    var touchEnd = myTouch.position;
					
                    //Calculate the difference between the beginning and end of the touch on the x axis.
                    var x = touchEnd.x - touchComponent.TouchOrigin.x;
					
                    //Calculate the difference between the beginning and end of the touch on the y axis.
                    var y = touchEnd.y - touchComponent.TouchOrigin.y;

                    //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
                    touchComponent.TouchOrigin = new Vector2(-1, touchComponent.TouchOrigin.y);
					
                    //Check if the difference along the x axis is greater than the difference along the y axis.
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                        //If x is greater than zero, set horizontal to 1, otherwise set it to -1
                        horizontal = x > 0 ? 1 : -1;
                    else
                    //If y is greater than zero, set horizontal to 1, otherwise set it to -1
                        vertical = y > 0 ? 1 : -1;
                }
            }

            Debug.Log("INPUT: " + horizontal + " | " + vertical);
            if (horizontal != 0 || vertical != 0)
            {
                //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
                //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
                var movement = new Vector2(horizontal, vertical);
                movementComponent.Movement.Value = movement;
                Debug.Log("MOVING: " + movement);
            }
        }
    }
}