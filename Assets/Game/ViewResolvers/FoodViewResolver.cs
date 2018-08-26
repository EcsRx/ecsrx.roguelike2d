using System.Linq;
using Assets.Game.Components;
using Assets.Game.SceneCollections;
using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using EcsRx.Unity.Systems;
using EcsRx.Views.Components;
using EcsRx.Views.ViewHandlers;
using UnityEngine;
using Zenject;

namespace Assets.Game.ViewResolvers
{
    [Priority(2)]
    public class FoodViewResolver : PrefabViewResolverSystem
    {
        private readonly FoodTiles _foodTiles;

        public IGroup Group { get; } = new Group(typeof(FoodComponent), typeof(ViewComponent));

        public FoodViewResolver(IEntityCollectionManager collectionManager, IEventSystem eventSystem, IInstantiator instantiator, FoodTiles foodTiles) : base(collectionManager, eventSystem, instantiator)
        {
            _foodTiles = foodTiles;
        }


        public override GameObject ResolveView(IEntity entity)
        {
            var foodComponent = entity.GetComponent<FoodComponent>();
            var foodTileIndex = foodComponent.IsSoda ? 1 : 0;
            var tileChoice = _foodTiles.AvailableTiles.ElementAt(foodTileIndex);
            var gameObject = Object.Instantiate(tileChoice, Vector3.zero, Quaternion.identity) as GameObject;
            gameObject.name = string.Format("food-{0}", entity.Id);
            return gameObject;
        }

        protected override GameObject PrefabTemplate { get; }
        
        protected override void OnViewCreated(IEntity entity, GameObject view)
        {
            throw new System.NotImplementedException();
        }
    }
}