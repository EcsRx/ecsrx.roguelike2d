using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using EcsRx.Views.Components;
using Game.Components;
using Game.Extensions;
using Game.SceneCollections;
using UnityEngine;
using Zenject;

namespace Game.ViewResolvers
{
    [Priority(100)]
    public class ExitViewResolver : DynamicViewResolverSystem
    {
        private readonly ExitTiles _exitTiles;

        public ExitViewResolver(IEventSystem eventSystem, IEntityCollectionManager collectionManager, IUnityInstantiator instantiator, ExitTiles exitTiles) 
            : base(eventSystem, collectionManager, instantiator)
        { _exitTiles = exitTiles; }

        public override IGroup Group { get; } = new Group(typeof(ExitComponent), typeof(ViewComponent));
        
        public override GameObject CreateView(IEntity entity)
        {
            var tileChoice = _exitTiles.AvailableTiles.TakeRandom();
            var gameObject = Object.Instantiate(tileChoice, Vector3.zero, Quaternion.identity);
            gameObject.name = $"exit-{entity.Id}";
            return gameObject;
        }

        public override void DestroyView(IEntity entity, GameObject view)
        { GameObject.Destroy(view); }
    }
}