using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Systems
{
    public class LevelScreenVisibilitySystem : IReactToEntitySystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(LevelComponent));
        private readonly GameObject _levelImage;

        public IGroup TargetGroup { get { return _targetGroup; } }

        public LevelScreenVisibilitySystem()
        {
            _levelImage = GameObject.Find("LevelImage");
        }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            return entity.GetComponent<LevelComponent>().HasLoaded.DistinctUntilChanged().Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            var levelComponent = entity.GetComponent<LevelComponent>();
            _levelImage.SetActive(levelComponent.HasLoaded.Value);
        }
    }
}