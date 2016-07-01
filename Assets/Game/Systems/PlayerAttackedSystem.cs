using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using EcsRx.Systems.Custom;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class PlayerAttackedSystem : EventReactionSystem<PlayerHitEvent>
    {
        public PlayerAttackedSystem(IEventSystem eventSystem) : base(eventSystem) {}

        public override void EventTriggered(PlayerHitEvent eventData)
        {
            var playerComponent = eventData.Player.GetComponent<PlayerComponent>();
            playerComponent.Food.Value -= 10;

            var viewComponent = eventData.Enemy.GetComponent<ViewComponent>();
            var animator = viewComponent.View.GetComponent<Animator>();
            animator.SetTrigger("enemyAttack");
        }
    }
}