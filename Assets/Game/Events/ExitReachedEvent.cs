using EcsRx.Entities;

namespace Game.Events
{
    public class ExitReachedEvent
    {
        public IEntity Exit { get; private set; }
        public IEntity Player { get; private set; }

        public ExitReachedEvent(IEntity exit, IEntity player)
        {
            Exit = exit;
            Player = player;
        }
    }
}