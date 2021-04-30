using SystemsRx.Events;
using SystemsRx.Systems.Conventional;
using EcsRx.Extensions;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerAttackedSystem : IReactToEventSystem<PlayerHitEvent>
    {
        private IEventSystem _eventSystem;

        public PlayerAttackedSystem(IEventSystem eventSystem)
        { _eventSystem = eventSystem; }

        public void Process(PlayerHitEvent eventData)
        {
            var enemyComponent = eventData.Enemy.GetComponent<EnemyComponent>();
            var playerComponent = eventData.Player.GetComponent<PlayerComponent>();
            playerComponent.Food.Value -= enemyComponent.EnemyPower;

            var animator = eventData.Enemy.GetUnityComponent<Animator>();
            animator.SetTrigger("enemyAttack");

            if (playerComponent.Food.Value <= 0)
            { _eventSystem.Publish(new PlayerKilledEvent(eventData.Player)); }
        }
    }
}