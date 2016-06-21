using Assets.EcsRx.Unity.Extensions;
using Assets.Game.Components;
using Assets.Game.Groups;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;

namespace Assets.Game.Systems
{
    public class WallDestroyedSystem : IReactToEntitySystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(WallComponent));

        public IGroup TargetGroup { get { return _targetGroup; } }
        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            return entity.GetComponent<WallComponent>().Health.Where(x => x <= 0).Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            var viewComponent = entity.GetComponent<ViewComponent>();
            viewComponent.DestroyView();
        }
    }
}