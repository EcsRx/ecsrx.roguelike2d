using EcsRx.Entities;

namespace Game.Events
{
    public class EnemyTurnEvent
    {
        public IEntity Enemy { get; private set; }

        public EnemyTurnEvent(IEntity enemy)
        {
            Enemy = enemy;
        }
    }
}