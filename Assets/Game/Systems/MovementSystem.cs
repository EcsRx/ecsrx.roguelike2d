using System;
using System.Collections;
using Assets.Game.Components;
using Assets.Game.Configuration;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class MovementSystem : IReactToEntitySystem
    {
        private LayerMask _blockingLayer = LayerMask.GetMask("BlockingLayer");
        private IGroup _targetGroup = new Group(typeof(ViewComponent), typeof(MovementComponent));
        private GameConfiguration _gameConfiguration;

        public IGroup TargetGroup { get { return _targetGroup; } }

        public MovementSystem(GameConfiguration gameConfiguration)
        {
            _gameConfiguration = gameConfiguration;
        }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var movementComponent = entity.GetComponent<MovementComponent>();
            return movementComponent.Movement.DistinctUntilChanged().Where(x => x != Vector2.zero).Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            var view = entity.GetComponent<ViewComponent>().View;
            var movementComponent = entity.GetComponent<MovementComponent>();

            Vector2 currentPosition = view.transform.position;
            var destination = currentPosition + movementComponent.Movement.Value;
            var canMove = CanMove(view, currentPosition, destination);
            var isPlayer = entity.HasComponent<PlayerComponent>();

            if (canMove && isPlayer)
            {
                var rigidBody = view.GetComponent<Rigidbody2D>();
//                SmoothMovement(view, rigidBody, destination);
                MainThreadDispatcher.StartUpdateMicroCoroutine(SmoothMovement(view, rigidBody, destination));
                movementComponent.Movement.Value = Vector2.zero;
            }
        }

        private bool CanMove(GameObject mover, Vector2 start, Vector2 destination)
        {
            var boxCollider = mover.GetComponent<BoxCollider2D>();
            boxCollider.enabled = false;
            var hit = Physics2D.Linecast(start, destination, _blockingLayer);
            boxCollider.enabled = true;
            return (hit.transform == null);
        }

        protected void SmoothMovement1(GameObject mover, Rigidbody2D rigidBody, Vector3 destination)
        { 
            Observable.EveryUpdate()
                .TakeWhile(x => (mover.transform.position - destination).sqrMagnitude > float.Epsilon)
                .Do(x => {
                    Debug.Log("STILL MOVING");
                    var newPostion = Vector3.MoveTowards(rigidBody.position, destination, _gameConfiguration.MovementSpeed * Time.deltaTime);
                    rigidBody.MovePosition(newPostion);
                });
        }

        protected IEnumerator SmoothMovement(GameObject mover, Rigidbody2D rigidBody, Vector3 destination)
        {
            Debug.Log("Moving To: " + destination);
            while (Vector3.Distance(mover.transform.position, destination) > 0.1f)
            {
                var newPostion = Vector3.MoveTowards(rigidBody.position, destination, _gameConfiguration.MovementSpeed * Time.deltaTime);
                rigidBody.MovePosition(newPostion);
                yield return null;
            }
            mover.transform.position = destination;
        }
    }
}