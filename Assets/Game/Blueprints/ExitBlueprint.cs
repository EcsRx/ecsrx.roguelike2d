using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Views.Components;
using Game.Components;

namespace Game.Blueprints
{
    public class ExitBlueprint : IBlueprint
    {
        public void Apply(IEntity entity)
        {
            entity.AddComponents(new ExitComponent(), new ViewComponent(), new RandomlyPlacedComponent());
        }
    }
}