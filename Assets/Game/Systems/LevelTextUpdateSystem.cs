using System;
using Assets.Game.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Systems
{
    public class LevelTextUpdateSystem : IReactToEntitySystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(LevelComponent));
        
        private readonly Text _levelText;

        public IGroup TargetGroup { get { return _targetGroup; } }

        public LevelTextUpdateSystem()
        {
            _levelText = GameObject.Find("LevelText").GetComponent<Text>();
        }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var levelComponent = entity.GetComponent<LevelComponent>();
            return levelComponent.Level.DistinctUntilChanged().Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            var levelComponent = entity.GetComponent<LevelComponent>();
            _levelText.text = string.Format("Day {0}", levelComponent.Level.Value);
        }
    }
}