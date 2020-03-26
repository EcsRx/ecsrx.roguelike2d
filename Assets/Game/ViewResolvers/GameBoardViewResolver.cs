using EcsRx.Attributes;
using EcsRx.Collections;
using EcsRx.Collections.Database;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Unity.Dependencies;
using EcsRx.Unity.Systems;
using Game.Components;
using Game.Extensions;
using Game.Groups;
using Game.SceneCollections;
using UnityEngine;
using Zenject;

namespace Game.ViewResolvers
{
    [Priority(100)]
    public class GameBoardViewResolver : DynamicViewResolverSystem
    {
        private readonly FloorTiles _floorTiles;
        private readonly OuterWallTiles _outerWallTiles;

        public override IGroup Group { get; } = new GameBoardGroup();
        
        public GameBoardViewResolver(IEventSystem eventSystem, IEntityDatabase entityDatabase, 
            IUnityInstantiator instantiator, FloorTiles floorTiles, OuterWallTiles outerWallTiles) 
            : base(eventSystem, entityDatabase, instantiator)
        {
            _floorTiles = floorTiles;
            _outerWallTiles = outerWallTiles;
        }
        
        public override GameObject CreateView(IEntity entity)
        {
            var rootView = new GameObject("Board");
            var boardComponent = entity.GetComponent<GameBoardComponent>();
            CreateBoardTiles(rootView.transform, boardComponent.Width, boardComponent.Height);
            return rootView;
        }

        public override void DestroyView(IEntity entity, GameObject view)
        { GameObject.Destroy(view); }

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

                    var instance = Object.Instantiate(tileToInstantiate, new Vector3(x, y, 0f), Quaternion.identity);
                    instance.name = $"game-tile-{index}";
                    instance.transform.SetParent(parentContainer);
                    index++;
                }
            }
        }
    }
}