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
    public class EnemyViewResolver : ViewResolverSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(EnemyComponent), typeof(ViewComponent));
        private readonly EnemyTiles _enemyTiles;

        public override IGroup TargetGroup
        {
            get { return _targetGroup; }
        }

        public EnemyViewResolver(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator, EnemyTiles enemyTiles) : base(poolManager, eventSystem, instantiator)
        {
            _enemyTiles = enemyTiles;
        }

        public override GameObject ResolveView(IEntity entity)
        {
            var enemyComponent = entity.GetComponent<EnemyComponent>();
            var enemyType = (int)enemyComponent.EnemyType;
            var tileChoice = _enemyTiles.AvailableTiles.ElementAt(enemyType);
            var gameObject = Object.Instantiate(tileChoice, Vector3.zero, Quaternion.identity) as GameObject;
            gameObject.name = string.Format("enemy-{0}", entity.Id);
            return gameObject;
        }
    }
}