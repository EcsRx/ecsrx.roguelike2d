using Assets.Game.Components;
using EcsRx.Blueprints;
using EcsRx.Entities;
using UnityEngine;

namespace Assets.Game.Blueprints
{
    public class LevelBlueprint : IBlueprint
    {
        private readonly int _minWalls = 4;
        private readonly int _maxWalls = 9;
        private readonly int _minFood = 1;
        private readonly int _maxFood = 5;
        private readonly int _level;

        public LevelBlueprint(int level = 1)
        {
            _level = level;
        }

        public void Apply(IEntity entity)
        {
            var levelComponent = new LevelComponent();
            UpdateLevel(levelComponent, _level);
            entity.AddComponents(levelComponent);
        }

        public void UpdateLevel(LevelComponent levelComponent, int level)
        {
            levelComponent.EnemyCount = (int) Mathf.Log(level, 2f);
            levelComponent.FoodCount = Random.Range(_minFood, _maxFood);
            levelComponent.WallCount = Random.Range(_minWalls, _maxWalls);
            levelComponent.Level.Value = level;
        }
    }
}