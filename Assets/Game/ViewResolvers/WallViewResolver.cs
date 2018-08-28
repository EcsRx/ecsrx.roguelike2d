using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Unity.Systems;
using EcsRx.Views.Components;
using Game.Components;
using Game.Extensions;
using Game.SceneCollections;
using UnityEngine;
using Zenject;

namespace Game.ViewResolvers
{
    [Priority(2)]
    public class WallViewResolver : DynamicViewResolverSystem
    {
        private readonly WallTiles _wallTiles;
        
        public override IGroup Group { get; } = new Group(typeof(WallComponent), typeof(ViewComponent));

        public WallViewResolver(IEventSystem eventSystem, IEntityCollectionManager collectionManager, IInstantiator instantiator, WallTiles wallTiles) 
            : base(eventSystem, collectionManager, instantiator)
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