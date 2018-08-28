using System.Collections.Generic;
using EcsRx.Components;
using UnityEngine;

namespace Game.Components
{
    public class GameBoardComponent : IComponent
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public IList<Vector3> OpenTiles { get; set; }

        public GameBoardComponent()
        {
            OpenTiles = new List<Vector3>();
        }
    }
}