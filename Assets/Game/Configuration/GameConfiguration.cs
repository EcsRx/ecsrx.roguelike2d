namespace Assets.Game.Configuration
{
    public class GameConfiguration
    {
        public float IntroLength { get; private set; }  
        public float TurnDelay { get; private set; }  
        public float MovementSpeed { get; private set; }
        public float MovementTime { get; private set; }
        public int StartingFoodPoints { get; private set; }

        public GameConfiguration()
        {
            IntroLength = 0.2f;
            TurnDelay = 0.1f;
            MovementSpeed = 5.0f;
            MovementTime = 0.1f;
            StartingFoodPoints = 100;
        }
    }
}