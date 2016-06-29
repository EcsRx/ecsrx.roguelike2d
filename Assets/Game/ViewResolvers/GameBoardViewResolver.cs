using Assets.EcsRx.Framework.Attributes;
using Assets.Game.Components;
using Assets.Game.Extensions;
using Assets.Game.Groups;
using Assets.Game.SceneCollections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Unity.Systems;
using UnityEngine;
using Zenject;

namespace Assets.Game.ViewResolvers
{
    [Priority(1)]
    public class GameBoardViewResolver : ViewResolverSystem
    {
        private readonly IGroup _targetGroup = new GameBoardGroup();
        private readonly FloorTiles _floorTiles;
        private readonly OuterWallTiles _outerWallTiles;

        public override IGroup TargetGroup
        {
            get { return _targetGroup; }
        }

        public GameBoardViewResolver(IPoolManager poolManager, IEventSystem eventSystem, IInstantiator instantiator, 
            FloorTiles floorTiles, OuterWallTiles wallTiles) : base(poolManager, eventSystem, instantiator)
        {
            _floorTiles = floorTiles;
            _outerWallTiles = wallTiles;
        }

        public override GameObject ResolveView(IEntity entity)
        {
            var rootView = new GameObject("Board");
            var boardComponent = entity.GetComponent<GameBoardComponent>();
            CreateBoardTiles(rootView.transform, boardComponent.Width, boardComponent.Height);

            return rootView;
        }

        private void CreateBoardTiles(Transform parentContainer, int width, int height)
        {
            var index = 0;
            for (var x = -1; x < width + 1; x++)
            {
                for (var y = -1; y < height + 1; y++)
                {
                    var tileToInstantiate = _floorTiles.AvailableTiles.TakeRandom();

                    if (x == -1 || x == width || y == -1 || y == height)
                    { tileToInstantiate = _outerWallTiles.AvailableTiles.TakeRandom(); }

                    var instance = Object.Instantiate(tileToInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    instance.name = string.Format("game-tile-{0}", index);
                    instance.transform.SetParent(parentContainer);
                    index++;
                }
            }
        }
    }
}