using EcsRx.Entities;

namespace Game.Events
{
    public class FoodPickupEvent
    {
        public IEntity Food { get; private set; } 
        public IEntity Player { get; private set; }
        public bool IsSoda { get; private set; }

        public FoodPickupEvent(IEntity food, IEntity player, bool isSoda)
        {
            Food = food;
            Player = player;
            IsSoda = isSoda;
        }
    }
}