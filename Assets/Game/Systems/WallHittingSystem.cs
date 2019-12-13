﻿using EcsRx.Collections;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Plugins.ReactiveSystems.Custom;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class WallHitSystem : EventReactionSystem<WallHitEvent>
    {
        private readonly IEntityCollectionManager _entityCollectionManager;

        public WallHitSystem(IEventSystem eventSystem, IEntityCollectionManager entityCollectionManager) : base(eventSystem)
        { _entityCollectionManager = entityCollectionManager; }

        public override void EventTriggered(WallHitEvent eventData)
        {
            var wallComponent = eventData.Wall.GetComponent<WallComponent>();
            wallComponent.Health.Value--;

            var animator = eventData.Player.GetUnityComponent<Animator>();
            animator.SetTrigger("playerChop");

            if (wallComponent.Health.Value <= 0)
            { _entityCollectionManager.RemoveEntity(eventData.Wall); }
        }
    }
}