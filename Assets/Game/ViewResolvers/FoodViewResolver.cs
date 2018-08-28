using System.Linq;
using Assets.Game.Components;
using Assets.Game.SceneCollections;
using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Systems;
using EcsRx.Views.Components;
using UnityEngine;
using Zenject;

namespace Assets.Game.ViewResolvers
{
    [Priority(2)]
    public class FoodViewResolver : DynamicViewResolverSystem
    {
        private readonly FoodTiles _foodTiles;
        
        public override IGroup Group { get; } = new Group(typeof(FoodComponent), typeof(ViewComponent));
        
        public FoodViewResolver(IEventSystem eventSystem, IEntityCollectionManager collectionManager, IInstantiator instantiator, FoodTiles foodTiles) 
            : base(eventSystem, collectionManager, instantiator)
        {
            _foodTiles = foodTiles;
        }
        
        public override GameObject CreateView(IEntity entity)
        {
            var foodComponent = entity.GetComponent<FoodComponent>();
            var foodTileIndex = foodComponent.IsSoda ? 1 : 0;
            var tileChoice = _foodTiles.AvailableTiles.ElementAt(foodTileIndex);
            var gameObject = Object.Instantiate(tileChoice, Vector3.zero, Quaternion.identity);
            gameObject.name = $"food-{entity.Id}";
            return gameObject;
        }

        public override void DestroyView(IEntity entity, GameObject view)
        { GameObject.Destroy(view); }
    }
}