using Assets.EcsRx.Framework.Blueprints;
using Assets.Game.Components;
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

        public LevelBlueprint(int level = 3)
        {
            _level = level;
        }

        public void Apply(IEntity entity)
        {
            var levelComponent = new LevelComponent
            {
                EnemyCount = (int)Mathf.Log(_level, 2f),
                FoodCount = Random.Range(_minFood, _maxFood),
                WallCount = Random.Range(_minWalls, _maxWalls)
            };

            levelComponent.Level.Value = _level;

            entity.AddComponent(levelComponent);
        }
    }
}