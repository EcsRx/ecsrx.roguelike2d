using SystemsRx.Systems.Conventional;
using EcsRx.Collections.Database;
using EcsRx.Extensions;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UnityEngine;

namespace Game.Systems
{
    public class WallHitSystem : IReactToEventSystem<WallHitEvent>
    {
        private readonly IEntityDatabase _entityDatabase;

        public WallHitSystem(IEntityDatabase entityDatabase)
        { _entityDatabase = entityDatabase; }

        public void Process(WallHitEvent eventData)
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