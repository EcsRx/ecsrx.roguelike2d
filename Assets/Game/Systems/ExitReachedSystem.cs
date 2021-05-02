using System.Linq;
using SystemsRx.Systems.Conventional;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Plugins.GroupBinding.Attributes;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using Game.Blueprints;
using Game.Components;
using Game.Events;
using UniRx;

namespace Game.Systems
{
    public class ExitReachedSystem : IReactToEventSystem<ExitReachedEvent>, IManualSystem, IGroupSystem
    {
        public IGroup Group { get; } = new Group(typeof(LevelComponent));

        [FromGroup]
        public IObservableGroup ObservableGroup;

        private IEntity _level;
        

        public void StartSystem()
        { this.WaitForScene().Subscribe(x => _level = ObservableGroup.First()); }

        public void StopSystem()
        {}

        public void Process(ExitReachedEvent eventData)
        {
            var movementComponent = eventData.Player.GetComponent<MovementComponent>();
            movementComponent.StopMovement = true;

            var levelComponent = _level.GetComponent<LevelComponent>();
            var currentLevel = levelComponent.Level.Value;
            var levelBlueprint = new LevelBlueprint();
            levelBlueprint.UpdateLevel(levelComponent, currentLevel + 1);
        }
    }
}