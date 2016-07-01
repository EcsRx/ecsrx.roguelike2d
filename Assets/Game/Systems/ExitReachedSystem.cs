using System.Linq;
using Assets.Game.Blueprints;
using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems.Custom;
using UniRx;

namespace Assets.Game.Systems
{
    public class ExitReachedSystem : EventReactionSystem<ExitReachedEvent>
    {
        public override IGroup TargetGroup { get { return new Group(typeof(LevelComponent)); } }
        private IEntity _level;

        public ExitReachedSystem(IEventSystem eventSystem) : base(eventSystem)
        {}

        public override void StartSystem(GroupAccessor @group)
        {
            base.StartSystem(@group);
            this.WaitForScene().Subscribe(x => _level = @group.Entities.First());
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