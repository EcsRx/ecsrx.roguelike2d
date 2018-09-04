using EcsRx.Entities;

namespace Game.Events
{
    public class WallHitEvent
    {
        public IEntity Wall { get; private set; } 
        public IEntity Player { get; private set; }

        public WallHitEvent(IEntity wall, IEntity player)
        {
            Wall = wall;
            Player = player;
        }
    }
}