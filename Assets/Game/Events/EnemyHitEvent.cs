using EcsRx.Entities;

namespace Game.Events
{
    public class EnemyHitEvent
    {
        public IEntity Enemy { get; private set; }
        public IEntity Player { get; private set; }

        public EnemyHitEvent(IEntity enemy, IEntity player)
        {
            Enemy = enemy;
            Player = player;
        }
    }
}