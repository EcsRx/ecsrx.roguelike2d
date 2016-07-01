using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Events;
using EcsRx.Systems.Custom;
using EcsRx.Unity.Components;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class PlayerAttackedSystem : EventReactionSystem<PlayerHitEvent>
    {
        public PlayerAttackedSystem(IEventSystem eventSystem) : base(eventSystem) {}

        public override void EventTriggered(PlayerHitEvent eventData)
        {
            var enemyComponent = eventData.Enemy.GetComponent<EnemyComponent>();
            var playerComponent = eventData.Player.GetComponent<PlayerComponent>();
            playerComponent.Food.Value -= enemyComponent.EnemyPower;

            var viewComponent = eventData.Enemy.GetComponent<ViewComponent>();
            var animator = viewComponent.View.GetComponent<Animator>();
            animator.SetTrigger("enemyAttack");

            if (playerComponent.Food.Value <= 0)
            {
                EventSystem.Publish(new PlayerKilledEvent(eventData.Player));
            }
        }
    }
}