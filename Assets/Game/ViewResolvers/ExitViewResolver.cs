using Assets.Game.Components;
using Assets.Game.Extensions;
using Assets.Game.SceneCollections;
using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Unity.Systems;
using EcsRx.Views.Components;
using UnityEngine;
using Zenject;

namespace Assets.Game.ViewResolvers
{
    [Priority(2)]
    public class ExitViewResolver : DynamicViewResolverSystem
    {
        private readonly ExitTiles _exitTiles;

        public ExitViewResolver(IEventSystem eventSystem, IEntityCollectionManager collectionManager, IInstantiator instantiator, ExitTiles exitTiles) 
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