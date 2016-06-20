using Assets.EcsRx.Framework.Blueprints;
using Assets.Game.Components;
using EcsRx.Entities;
using EcsRx.Unity.Components;

namespace Assets.Game.Blueprints
{
    public class WallBlueprint : IBlueprint
    {
        public void Apply(IEntity entity)
        {
            entity.AddComponent<WallComponent>();
            entity.AddComponent<ViewComponent>();
            entity.AddComponent<RandomlyPlacedComponent>();
        }
    }
}