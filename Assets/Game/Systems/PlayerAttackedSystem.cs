using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Plugins.ReactiveSystems.Custom;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class PlayerAttackedSystem : EventReactionSystem<PlayerHitEvent>
    {
        public PlayerAttackedSystem(IEventSystem eventSystem) : base(eventSystem) {}

        public override void EventTriggered(PlayerHitEvent eventData)
        {
            var enemyComponent = eventData.Enemy.GetComponent<EnemyComponent>();
            var playerComponent = eventData.Player.GetComponent<PlayerComponent>();
            playerComponent.Food.Value -= enemyComponent.EnemyPower;

            var animator = eventData.Enemy.GetUnityComponent<Animator>();
            animator.SetTrigger("enemyAttack");

            if (playerComponent.Food.Value <= 0)
            { EventSystem.Publish(new PlayerKilledEvent(eventData.Player)); }
        }
    }
}