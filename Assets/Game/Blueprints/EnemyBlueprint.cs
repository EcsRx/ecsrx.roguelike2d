using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using Game.Components;
using Game.Enums;
using Random = UnityEngine.Random;

namespace Game.Blueprints
{
    public class EnemyBlueprint : IBlueprint
    {
        private EnemyTypes GetRandomEnemyType()
        {
            var enemyValue = Random.Range(0, 2); // Its exclusive on max, ask unity...
            return (EnemyTypes) enemyValue;
        }

        public void Apply(IEntity entity)
        {
            var enemyComponent = new EnemyComponent();
            enemyComponent.Health.Value = 3;
            enemyComponent.EnemyType = GetRandomEnemyType();
            enemyComponent.EnemyPower = enemyComponent.EnemyType == EnemyTypes.Regular ? 10 : 20;
            entity.AddComponents(enemyComponent, new ViewComponent(),
                new MovementComponent(), new RandomlyPlacedComponent());
        }
    }
}