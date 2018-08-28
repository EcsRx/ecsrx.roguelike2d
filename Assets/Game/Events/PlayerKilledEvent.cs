using EcsRx.Entities;

namespace Game.Events
{
    public class PlayerKilledEvent
    {
        public IEntity Player { get; private set; }

        public PlayerKilledEvent(IEntity player)
        {
            Player = player;
        }
    }
}