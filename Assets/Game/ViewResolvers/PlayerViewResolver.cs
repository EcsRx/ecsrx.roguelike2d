using Assets.Game.Components;
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
    public class PlayerViewResolver : ViewResolverSystem
    {
        private IGroup _targetGroup = new Group(typeof(PlayerComponent), typeof(ViewComponent));

        public override IGroup TargetGroup
        {
            get { return _targetGroup; }
        }

        public PlayerViewResolver(IViewHandler viewHandler) : base(viewHandler) {}

        public override GameObject ResolveView(IEntity entity)
        {
            var playerPrefab = Resources.Load("Prefabs/Player") as GameObject;
            var gameObject = Object.Instantiate(playerPrefab);
            gameObject.name = "Player";
            return gameObject;
        }
    }
}