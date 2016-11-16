using Assets.Game.Components;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Unity.Components;
using UnityEngine;

namespace Assets.Game.Blueprints
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
            entity.AddComponent(foodComponent);
            entity.AddComponent<ViewComponent>();
            entity.AddComponent<RandomlyPlacedComponent>();
        }
    }
}