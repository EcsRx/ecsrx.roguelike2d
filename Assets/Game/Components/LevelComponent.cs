using System;
using EcsRx.Components;
using UniRx;

namespace Assets.Game.Components
{
    public class LevelComponent : IComponent, IDisposable
    {
        public ReactiveProperty<bool> HasLoaded { get; set; } 
        public ReactiveProperty<int> Level { get; set; }
        public int WallCount { get; set; }
        public int FoodCount { get; set; }
        public int EnemyCount { get; set; }

        public LevelComponent()
        {
            HasLoaded = new BoolReactiveProperty(false);
            Level = new IntReactiveProperty(1);
        }

        public void Dispose()
        {
            HasLoaded.Dispose();
            Level.Dispose();
        }
    }
}