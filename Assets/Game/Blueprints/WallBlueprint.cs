using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Views.Components;
using Game.Components;

namespace Game.Blueprints
{
    public class WallBlueprint : IBlueprint
    {
        private readonly int DefaultWallHealth = 3;

        public void Apply(IEntity entity)
        {
            var wallComponent = new WallComponent();
            wallComponent.Health.Value = DefaultWallHealth;
            entity.AddComponents(wallComponent, new ViewComponent(), new RandomlyPlacedComponent());
        }
    }
}