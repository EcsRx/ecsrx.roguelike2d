using Assets.EcsRx.Framework.Blueprints;
using Assets.Game.Components;
using EcsRx.Entities;
using EcsRx.Unity.Components;

namespace Assets.Game.Blueprints
{
    public class EnemyBlueprint : IBlueprint
    {
        public void Apply(IEntity entity)
        {
            entity.AddComponent<EnemyComponent>();
            entity.AddComponent<ViewComponent>();
            entity.AddComponent<RandomlyPlacedComponent>();
        }
    }
}