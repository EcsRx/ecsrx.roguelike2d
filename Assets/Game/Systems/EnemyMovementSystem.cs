using System.Linq;
using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class EnemyMovementSystem : IReactToEntitySystem
    {
        private readonly IEventSystem _eventSystem;
        private readonly GroupAccessor _playerAccessor;
        private readonly IGroup _targetGroup = new Group(typeof(MovementComponent), typeof(EnemyComponent));

        public IGroup TargetGroup { get { return _targetGroup; } }


        public EnemyMovementSystem(IEventSystem eventSystem, IPoolManager poolManager)
        {
            _eventSystem = eventSystem;
            _playerAccessor = poolManager.CreateGroupAccessor(new Group(typeof (PlayerComponent)));
        }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            return _eventSystem.Receive<EnemyTurnEvent>().Where(x => x.Enemy == entity).Select(x => entity);
        }

        public void Execute(IEntity entity)
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
            var entityLocation = entity.GetComponent<ViewComponent>().View.transform.position;
            movementComponent.Movement.Value = CalculateMovement(entityLocation, playerLocation);
        }

        private Vector3 GetPlayerLocation()
        {
            var player = _playerAccessor.Entities.First();
            return player.GetComponent<ViewComponent>().View.transform.position;
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