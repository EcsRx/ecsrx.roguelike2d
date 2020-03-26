using EcsRx.Collections;
using EcsRx.Collections.Database;
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
        private readonly IEntityDatabase _entityDatabase;

        public WallHitSystem(IEventSystem eventSystem, IEntityDatabase entityDatabase) : base(eventSystem)
        { _entityDatabase = entityDatabase; }

        public override void EventTriggered(WallHitEvent eventData)
        {
            var wallComponent = eventData.Wall.GetComponent<WallComponent>();
            wallComponent.Health.Value--;

            var animator = eventData.Player.GetUnityComponent<Animator>();
            animator.SetTrigger("playerChop");

            if (wallComponent.Health.Value <= 0)
            { _entityDatabase.RemoveEntity(eventData.Wall); }
        }
    }
}