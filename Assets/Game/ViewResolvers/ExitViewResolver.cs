using Assets.Game.Components;
using Assets.Game.Extensions;
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
    public class ExitViewResolver : ViewResolverSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(ExitComponent), typeof(ViewComponent));
        private readonly ExitTiles _exitTiles;

        public override IGroup TargetGroup
        {
            get { return _targetGroup; }
        }

        public ExitViewResolver(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator, ExitTiles exitTiles) : base(poolManager, eventSystem, instantiator)
        {
            _exitTiles = exitTiles;
        }

        public override GameObject ResolveView(IEntity entity)
        {
            var tileChoice = _exitTiles.AvailableTiles.TakeRandom();
            var gameObject = Object.Instantiate(tileChoice, Vector3.zero, Quaternion.identity) as GameObject;
            gameObject.name = string.Format("exit-{0}", entity.Id);
            return gameObject;
        }
    }
}