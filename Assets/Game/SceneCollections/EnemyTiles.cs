using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public class EnemyTiles
    {
        public IEnumerable<GameObject> AvailableTiles { get; private set; }

        public EnemyTiles()
        {
            AvailableTiles = new[]
            {
                Resources.Load<GameObject>("Prefabs/Enemy1"),
                Resources.Load<GameObject>("Prefabs/Enemy2")
            };
        }
    }
}