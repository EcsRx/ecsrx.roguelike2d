using SystemsRx.Systems.Conventional;
using EcsRx.Extensions;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Computeds;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class EnemyMovementSystem : IReactToEventSystem<EnemyTurnEvent>
    {
        private readonly IComputedPlayerPosition _computedPlayerPosition;

        public EnemyMovementSystem(IComputedPlayerPosition computedPlayerPosition)
        {
            _computedPlayerPosition = computedPlayerPosition;
        }

        public void Process(EnemyTurnEvent eventData)
        {
            var movementComponent = eventData.Enemy.GetComponent<MovementComponent>();
            if(movementComponent.Movement.Value != Vector2.zero) { return; }

            var enemyComponent = eventData.Enemy.GetComponent<EnemyComponent>();
            if (enemyComponent.IsSkippingNextTurn)
            {
                enemyComponent.IsSkippingNextTurn = false;
                return;
            }

            enemyComponent.IsSkippingNextTurn = true;

            var playerLocation = _computedPlayerPosition.Value;
            
            var gameObject = eventData.Enemy.GetGameObject();
            var entityLocation = gameObject.transform.position;
            movementComponent.Movement.Value = CalculateMovement(entityLocation, playerLocation);
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