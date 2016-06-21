using EcsRx.Entities;

namespace Assets.Game.Events
{
    public class WallHitEvent
    {
        public IEntity Wall { get; set; } 
    }
}