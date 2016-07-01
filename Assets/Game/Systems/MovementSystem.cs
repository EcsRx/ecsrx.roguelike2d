using System.Collections;
using Assets.Game.Components;
using Assets.Game.Configuration;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using EcsRx.Unity.MonoBehaviours;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class MovementSystem : IReactToEntitySystem
    {
        private readonly LayerMask _blockingLayer = LayerMask.GetMask("BlockingLayer");
        private readonly IGroup _targetGroup = new Group(typeof(ViewComponent), typeof(MovementComponent));
        private readonly GameConfiguration _gameConfiguration;
        private readonly IEventSystem _eventSystem;
        
        public IGroup TargetGroup { get { return _targetGroup; } }

        public MovementSystem(GameConfiguration gameConfiguration, IEventSystem eventSystem)
        {
            _gameConfiguration = gameConfiguration;
            _eventSystem = eventSystem;
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
            var collidedObject = CheckForCollision(view, currentPosition, destination);
            var canMove = collidedObject == null;

            var isPlayer = entity.HasComponent<PlayerComponent>();

            if (!canMove)
            {
                movementComponent.Movement.Value = Vector2.zero;

                var entityView = collidedObject.GetComponent<EntityView>();
                if(!entityView) { return; }

                if (isPlayer && collidedObject.tag.Contains("Wall"))
                { WallHit(entityView.Entity, entity); }

                if (isPlayer && collidedObject.tag.Contains("Enemy"))
                { EnemyHit(entityView.Entity, entity); }

                if(!isPlayer && collidedObject.tag.Contains("Player"))
                { PlayerHit(entityView.Entity, entity); }
                
                return;
            }

            var rigidBody = view.GetComponent<Rigidbody2D>();
            MainThreadDispatcher.StartUpdateMicroCoroutine(SmoothMovement(view, rigidBody, destination, movementComponent));
            _eventSystem.Publish(new EntityMovedEvent(isPlayer));

            if (isPlayer)
            {
                var playerComponent = entity.GetComponent<PlayerComponent>();
                playerComponent.Food.Value--;
            }
        }

        private GameObject CheckForCollision(GameObject mover, Vector2 start, Vector2 destination)
        {
            var boxCollider = mover.GetComponent<BoxCollider2D>();
            boxCollider.enabled = false;
            var hit = Physics2D.Linecast(start, destination, _blockingLayer);
            boxCollider.enabled = true;

            if(!hit.collider) { return null; }
            return hit.collider.gameObject;
        }

        protected IEnumerator SmoothMovement(GameObject mover, Rigidbody2D rigidBody, Vector3 destination, MovementComponent movementComponent)
        {
            while (Vector3.Distance(mover.transform.position, destination) > 0.1f)
            {
                if (mover == null || movementComponent.StopMovement)
                {
                    movementComponent.Movement.Value = Vector2.zero;
                    movementComponent.StopMovement = false;
                    yield break;
                }

                var newPostion = Vector3.MoveTowards(rigidBody.position, destination, _gameConfiguration.MovementSpeed * Time.deltaTime);
                rigidBody.MovePosition(newPostion);
                yield return null;
            }
            mover.transform.position = destination;
            movementComponent.Movement.Value = Vector2.zero;
        }

        private void WallHit(IEntity wall, IEntity player)
        {
            _eventSystem.Publish(new WallHitEvent(wall, player));
        }

        private void PlayerHit(IEntity player, IEntity enemy)
        {
            _eventSystem.Publish(new PlayerHitEvent(player, enemy));
        }

        private void EnemyHit(IEntity enemy, IEntity player)
        {
            _eventSystem.Publish(new EnemyHitEvent(enemy, player));
        }
    }
}