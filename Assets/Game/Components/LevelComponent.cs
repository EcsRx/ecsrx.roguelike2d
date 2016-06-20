using EcsRx.Components;
using UniRx;

namespace Assets.Game.Components
{
    public class LevelComponent : IComponent
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
    }
}