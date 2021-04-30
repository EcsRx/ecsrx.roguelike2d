using System.Linq;
using SystemsRx.Attributes;
using SystemsRx.Events;
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
    [Priority(100)]
    public class EnemyViewResolver : DynamicViewResolverSystem
    {
        private readonly EnemyTiles _enemyTiles;

        public override IGroup Group { get; } = new Group(typeof(EnemyComponent), typeof(ViewComponent));
       
        public EnemyViewResolver(IEventSystem eventSystem, IEntityDatabase entityDatabase, IUnityInstantiator instantiator, EnemyTiles enemyTiles)
            : base(eventSystem, entityDatabase, instantiator)
        {
            _enemyTiles = enemyTiles;
        }
        
        public override GameObject CreateView(IEntity entity)
        {
            var enemyComponent = entity.GetComponent<EnemyComponent>();
            var enemyType = (int)enemyComponent.EnemyType;
            var tileChoice = _enemyTiles.AvailableTiles.ElementAt(enemyType);
            var gameObject = Object.Instantiate(tileChoice, Vector3.zero, Quaternion.identity);
            gameObject.name = $"enemy-{entity.Id}";
            return gameObject;
        }

        public override void DestroyView(IEntity entity, GameObject view)
        { GameObject.Destroy(view); }
    }
}