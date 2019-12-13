using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using Game.Components;
using UnityEngine;

namespace Game.Blueprints
{
    public class FoodBlueprint : IBlueprint
    {
        private readonly int FoodValue = 10;
        private readonly int SodaValue = 20;

        private bool ShouldBeSoda()
        { return Random.Range(0, 2) == 1; }
    

        public void Apply(IEntity entity)
        {
            var foodComponent = new FoodComponent();
            var isSoda = ShouldBeSoda();
            foodComponent.IsSoda = isSoda;
            foodComponent.FoodAmount = isSoda ? SodaValue : FoodValue;
            entity.AddComponents(foodComponent, new ViewComponent(), new RandomlyPlacedComponent());
        }
    }
}