using System;
using System.Linq;
using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using EcsRx.Views.Components;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class EnemyMovementSystem : IReactToEntitySystem
    {
        private readonly IEventSystem _eventSystem;
        private readonly IObservableGroup _playerObservableGroup;

        public IGroup Group { get; } = new Group(typeof(MovementComponent), typeof(EnemyComponent));


        public EnemyMovementSystem(IEventSystem eventSystem, IEntityCollectionManager collectionManager)
        {
            _eventSystem = eventSystem;
            _playerObservableGroup = collectionManager.GetObservableGroup(new Group(typeof (PlayerComponent)));
        }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            return _eventSystem.Receive<EnemyTurnEvent>().Where(x => x.Enemy == entity).Select(x => entity);
        }

        public void Process(IEntity entity)
        {
            var movementComponent = entity.GetComponent<MovementComponent>();
            if(movementComponent.Movement.Value != Vector2.zero) { return; }

            var enemyComponent = entity.GetComponent<EnemyComponent>();
            if (enemyComponent.IsSkippingNextTurn)
            {
                enemyComponent.IsSkippingNextTurn = false;
                return;
            }

            enemyComponent.IsSkippingNextTurn = true;

            var playerLocation = GetPlayerLocation();
            var viewComponent = entity.GetComponent<ViewComponent>();
            var gameObject = viewComponent.View as GameObject;
            var entityLocation = gameObject.transform.position;
            movementComponent.Movement.Value = CalculateMovement(entityLocation, playerLocation);
        }

        private Vector3 GetPlayerLocation()
        {
            var player = _playerObservableGroup.First();
            var viewComponent = player.GetComponent<ViewComponent>();
            var gameObject = viewComponent.View as GameObject;
            return gameObject.transform.position;
        }

        private Vector2 CalculateMovement(Vector3 currentPosition, Vector3 playerPosition)
        {
            var x = 0.0f;
            var y = 0.0f;

            if (Mathf.Abs(playerPosition.x - currentPosition.x) < float.Epsilon)
            { y = playerPosition.y > currentPosition.y ? 1 : -1; }
            else
            { x = playerPosition.x > currentPosition.x ? 1 : -1; }

            return new Vector2(x, y);
        }
    }
}