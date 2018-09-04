namespace Game.Configuration
{
    public class GameConfiguration
    {
        public float IntroLength { get; private set; }  
        public float TurnDelay { get; private set; }  
        public float MovementSpeed { get; private set; }
        public float MovementTime { get; private set; }
        public int StartingFoodPoints { get; private set; }
        public int FoodValue { get; private set; }
        public int SodaValue { get; private set; }

        public GameConfiguration()
        {
            IntroLength = 2.0f;
            TurnDelay = 0.1f;
            MovementSpeed = 7.0f;
            MovementTime = 0.1f;
            StartingFoodPoints = 100;
            FoodValue = 10;
            SodaValue = 20;
        }
    }
}