using Assets.Game.Components;
using Assets.Game.Enums;
using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Views.Components;
using Random = UnityEngine.Random;

namespace Assets.Game.Blueprints
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