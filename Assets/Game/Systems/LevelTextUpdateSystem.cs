using System;
using System.Collections.Generic;
using System.Linq;
using SystemsRx.Extensions;
using SystemsRx.Events;
using SystemsRx.Systems.Conventional;
using EcsRx.Collections;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Plugins.GroupBinding.Attributes;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems
{
    public class LevelTextUpdateSystem : IManualSystem, IGroupSystem
    {
        public IGroup Group { get; } = new Group(typeof(LevelComponent));

        [FromGroup]
        public IObservableGroup ObservableGroup;
        
        private Text _levelText;
        private LevelComponent _levelComponent;
        private readonly IEventSystem _eventSystem;
        private readonly IList<IDisposable> _subscriptions = new List<IDisposable>();

        public LevelTextUpdateSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public void StartSystem()
        {
            this.WaitForScene()
                .Subscribe(x =>
                {
                    var level = ObservableGroup.First();
                    _levelComponent = level.GetComponent<LevelComponent>();
                    _levelText = GameObject.Find("LevelText").GetComponent<Text>();
                    SetupSubscriptions();
                });
        }

        private void SetupSubscriptions()
        {
            _levelComponent.Level.DistinctUntilChanged()
                .Subscribe(levelNumber => _levelText.text = $"Day {levelNumber}")
                .AddTo(_subscriptions);

            _eventSystem.Receive<PlayerKilledEvent>()
                .Subscribe(eventData => _levelText.text = $"After {_levelComponent.Level.Value} days, you starved.")
                .AddTo(_subscriptions);
        }

        public void StopSystem()
        { _subscriptions.DisposeAll(); }
    }
}