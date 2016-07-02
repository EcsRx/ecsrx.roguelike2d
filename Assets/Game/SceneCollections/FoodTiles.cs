using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public class FoodTiles
    {
        public IEnumerable<GameObject> AvailableTiles { get; private set; }

        public FoodTiles()
        {
            AvailableTiles = new[]
            {
                Resources.Load<GameObject>("Prefabs/Food"),
                Resources.Load<GameObject>("Prefabs/Soda"),
            };
        }
    }
}