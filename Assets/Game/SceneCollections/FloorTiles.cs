using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public class FloorTiles
    {
        public IEnumerable<GameObject> AvailableTiles { get; private set; }

        public FloorTiles()
        {
            AvailableTiles = new[]
            {
                Resources.Load<GameObject>("Prefabs/Floor1"),
                Resources.Load<GameObject>("Prefabs/Floor2"),
                Resources.Load<GameObject>("Prefabs/Floor3"),
                Resources.Load<GameObject>("Prefabs/Floor4"),
                Resources.Load<GameObject>("Prefabs/Floor5"),
                Resources.Load<GameObject>("Prefabs/Floor6"),
                Resources.Load<GameObject>("Prefabs/Floor7"),
                Resources.Load<GameObject>("Prefabs/Floor8")
            };
        }
    }
}