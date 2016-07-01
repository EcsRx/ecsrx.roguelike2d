using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Components;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Systems
{
    public class LevelTextUpdateSystem : IManualSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(LevelComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        private Text _levelText;
        private LevelComponent _levelComponent;
        private IList<IDisposable> _subscriptions = new List<IDisposable>();

        public void StartSystem(GroupAccessor @group)
        {
            this.WaitForScene()
                .Subscribe(x =>
                {
                    var level = @group.Entities.First();
                    _levelComponent = level.GetComponent<LevelComponent>();
                    _levelText = GameObject.Find("LevelText").GetComponent<Text>();
                    SetupSubscriptions();
                });
        }

        private void SetupSubscriptions()
        {
            _levelComponent.Level.DistinctUntilChanged()
                .Subscribe(levelNumber => _levelText.text = string.Format("Day {0}", levelNumber))
                .AddTo(_subscriptions);
        }

        public void StopSystem(GroupAccessor @group)
        {
            _subscriptions.DisposeAll();
        }
    }
}