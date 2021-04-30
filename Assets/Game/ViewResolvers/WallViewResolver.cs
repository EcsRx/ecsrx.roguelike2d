using SystemsRx.Attributes;
using SystemsRx.Events;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using EcsRx.Plugins.Views.Components;
using Game.Components;
using Game.Extensions;
using Game.SceneCollections;
using UnityEngine;

namespace Game.ViewResolvers
{
    [Priority(100)]
    public class WallViewResolver : DynamicViewResolverSystem
    {
        private readonly WallTiles _wallTiles;
        
        public override IGroup Group { get; } = new Group(typeof(WallComponent), typeof(ViewComponent));

        public WallViewResolver(IEventSystem eventSystem, IEntityDatabase entityDatabase, IUnityInstantiator instantiator, WallTiles wallTiles) 
            : base(eventSystem, entityDatabase, instantiator)
        {
            _wallTiles = wallTiles;
        }

        public override GameObject CreateView(IEntity entity)
        {
            var tileChoice = _wallTiles.AvailableTiles.TakeRandom();
            var gameObject = Object.Instantiate(tileChoice, Vector3.zero, Quaternion.identity) as GameObject;
            gameObject.name = $"wall-{entity.Id}";
            return gameObject;
        }

        public override void DestroyView(IEntity entity, GameObject view)
        { GameObject.Destroy(view); }
    }
}