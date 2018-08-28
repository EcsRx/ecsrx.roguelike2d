using System.Linq;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems.Custom;
using EcsRx.Unity.Extensions;
using Game.Blueprints;
using Game.Components;
using Game.Events;
using UniRx;

namespace Game.Systems
{
    public class ExitReachedSystem : EventReactionSystem<ExitReachedEvent>
    {
        private IEntity _level;
        
        public override IGroup Group { get; } = new Group(typeof(LevelComponent));

        public ExitReachedSystem(IEventSystem eventSystem) : base(eventSystem)
        {}

        public override void StartSystem(IObservableGroup group)
        {
            base.StartSystem(group);
            this.WaitForScene().Subscribe(x => _level = group.First());
        }

        public override void EventTriggered(ExitReachedEvent eventData)
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