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
using EcsRx.Systems.Custom;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;
using Game.Computeds;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class EnemyMovementSystem : EventReactionSystem<EnemyTurnEvent>
    {
        private readonly IComputedPlayerPosition _computedPlayerPosition;

        public EnemyMovementSystem(IEventSystem eventSystem, IComputedPlayerPosition computedPlayerPosition) 
            : base(eventSystem)
        {
            _computedPlayerPosition = computedPlayerPosition;
        }

        public override void EventTriggered(EnemyTurnEvent eventData)
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