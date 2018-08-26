using Assets.Game.Components;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Views.Components;

namespace Assets.Game.Blueprints
{
    public class ExitBlueprint : IBlueprint
    {
        public void Apply(IEntity entity)
        {
            entity.AddComponents(new ExitComponent(), new ViewComponent(), new RandomlyPlacedComponent());
        }
    }
}