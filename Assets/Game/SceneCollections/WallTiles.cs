using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public class WallTiles
    {
        public IEnumerable<GameObject> AvailableTiles { get; private set; }

        public WallTiles()
        {
            AvailableTiles = new[]
            {
                Resources.Load<GameObject>("Prefabs/Wall1"),
                Resources.Load<GameObject>("Prefabs/Wall2"),
                Resources.Load<GameObject>("Prefabs/Wall3"),
                Resources.Load<GameObject>("Prefabs/Wall4"),
                Resources.Load<GameObject>("Prefabs/Wall5"),
                Resources.Load<GameObject>("Prefabs/Wall6"),
                Resources.Load<GameObject>("Prefabs/Wall7"),
                Resources.Load<GameObject>("Prefabs/Wall8")
            };
        }
    }
}