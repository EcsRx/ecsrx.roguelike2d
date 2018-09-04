using System.Collections.Generic;
using UnityEngine;

namespace Game.SceneCollections
{
    public class OuterWallTiles
    {
        public IEnumerable<GameObject> AvailableTiles { get; private set; }

        public OuterWallTiles()
        {
            AvailableTiles = new[]
            {
                Resources.Load<GameObject>("Prefabs/OuterWall1"),
                Resources.Load<GameObject>("Prefabs/OuterWall2"),
                Resources.Load<GameObject>("Prefabs/OuterWall3")
            };
        }
    }
}