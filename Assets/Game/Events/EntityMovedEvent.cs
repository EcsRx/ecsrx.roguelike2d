namespace Game.Events
{
    public class EntityMovedEvent
    {
         public bool IsPlayer { get; private set; }

        public EntityMovedEvent(bool isPlayer)
        {
            IsPlayer = isPlayer;
        }
    }
}