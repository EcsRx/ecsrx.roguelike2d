using EcsRx.Entities;

namespace Assets.Game.Events
{
    public class PlayerHitEvent
    {
        public IEntity Enemy { get; private set; }
        public IEntity Player { get; private set; }

        public PlayerHitEvent(IEntity player, IEntity enemy)
        {
            Enemy = enemy;
            Player = player;
        }
    }
}