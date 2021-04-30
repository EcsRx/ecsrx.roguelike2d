using System.Linq;
using SystemsRx.Attributes;
using SystemsRx.Events;
using SystemsRx.Types;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using EcsRx.Plugins.Views.Components;
using Game.Components;
using Game.SceneCollections;
using UnityEngine;

namespace Game.ViewResolvers
{
    [Priority(PriorityTypes.Higher)]
    public class FoodViewResolver : DynamicViewResolverSystem
    {
        private readonly FoodTiles _foodTiles;
        
        public override IGroup Group { get; } = new Group(typeof(FoodComponent), typeof(ViewComponent));
        
        public FoodViewResolver(IEventSystem eventSystem, IEntityDatabase entityDatabase, IUnityInstantiator instantiator, FoodTiles foodTiles) 
            : base(eventSystem, entityDatabase, instantiator)
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