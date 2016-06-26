using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;

namespace Assets.Game.Systems
{
    public class EnemyMovementSystem : IReactToEntitySystem
    {
        private IEventSystem _eventSystem;
        private IGroup _targetGroup = new Group(typeof(MovementComponent), typeof(EnemyComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        public EnemyMovementSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            return _eventSystem.Receive<EnemyTurnEvent>().Where(x => x.Enemy == entity).Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}