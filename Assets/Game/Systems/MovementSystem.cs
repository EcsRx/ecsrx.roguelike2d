using System;
using System.Collections;
using SystemsRx.Events;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Extensions;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Plugins.Views.Components;
using EcsRx.Systems;
using Game.Components;
using Game.Configuration;
using Game.Events;
using UniRx;
using UnityEngine;

namespace Game.Systems
{
    public class MovementSystem : IReactToEntitySystem
    {
        private readonly LayerMask _blockingLayer = LayerMask.GetMask("BlockingLayer");
        
        private readonly GameConfiguration _gameConfiguration;
        private readonly IEventSystem _eventSystem;
        
        public IGroup Group { get; } = new Group(typeof(ViewComponent), typeof(MovementComponent)); 

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

        public void Process(IEntity entity)
        {
            var viewGameObject = entity.GetGameObject();
            var movementComponent = entity.GetComponent<MovementComponent>();

            Vector2 currentPosition = viewGameObject.transform.position;
            var destination = currentPosition + movementComponent.Movement.Value;
            var collidedObject = CheckForCollision(viewGameObject, currentPosition, destination);
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

            var rigidBody = viewGameObject.GetComponent<Rigidbody2D>();
            MainThreadDispatcher.StartUpdateMicroCoroutine(SmoothMovement(viewGameObject, rigidBody, destination, movementComponent));
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
            while (mover != null && Vector3.Distance(mover.transform.position, destination) > 0.1f)
            {
                if (movementComponent.StopMovement)
                {
                    movementComponent.Movement.Value = Vector2.zero;
                    movementComponent.StopMovement = false;
                    yield break;
                }

                var newPostion = Vector3.MoveTowards(rigidBody.position, destination, _gameConfiguration.MovementSpeed * Time.deltaTime);
                rigidBody.MovePosition(newPostion);
                yield return null;
            }

            if(mover != null)
            { mover.transform.position = destination; }

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