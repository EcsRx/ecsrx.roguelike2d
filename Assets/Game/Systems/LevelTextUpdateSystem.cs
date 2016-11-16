using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Events;
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
        private readonly IEventSystem _eventSystem;
        private readonly IList<IDisposable> _subscriptions = new List<IDisposable>();

        public LevelTextUpdateSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public void StartSystem(IGroupAccessor @group)
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

            _eventSystem.Receive<PlayerKilledEvent>()
                .Subscribe(eventData => _levelText.text = string.Format("After {0} days, you starved.", _levelComponent.Level.Value))
                .AddTo(_subscriptions);
        }

        public void StopSystem(IGroupAccessor @group)
        {
            _subscriptions.DisposeAll();
        }
    }
}