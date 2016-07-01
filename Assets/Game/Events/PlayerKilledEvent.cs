using EcsRx.Entities;

namespace Assets.Game.Events
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