using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Plugins.Views.Components;
using Game.Components;

namespace Game.Blueprints
{
    public class PlayerBlueprint : IBlueprint
    {
        private readonly int _playerFood;

        public PlayerBlueprint(int playerFood)
        {
            _playerFood = playerFood;
        }

        public void Apply(IEntity entity)
        {
            var playerComponent = new PlayerComponent();
            playerComponent.Food.Value = _playerFood;
            entity.AddComponents(playerComponent, new ViewComponent(), new MovementComponent());

#if UNITY_STANDALONE || UNITY_WEBPLAYER
            entity.AddComponent<StandardInputComponent>();
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            entity.AddComponent<TouchInputComponent>();
#endif
        }
    }
}