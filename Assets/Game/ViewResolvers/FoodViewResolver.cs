using System.Linq;
using Assets.Game.Components;
using Assets.Game.SceneCollections;
using EcsRx.Attributes;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Components;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Game.ViewResolvers
{
    [Priority(2)]
    public class FoodViewResolver : ViewResolverSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(FoodComponent), typeof(ViewComponent));
        private readonly FoodTiles _foodTiles;

        public override IGroup TargetGroup
        {
            get { return _targetGroup; }
        }

        public FoodViewResolver(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator, FoodTiles foodTiles) : base(poolManager, eventSystem, instantiator)
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
    }
}