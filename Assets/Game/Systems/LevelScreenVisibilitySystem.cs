using System;
using System.Collections.Generic;
using System.Linq;
using SystemsRx.Events;
using SystemsRx.Systems.Conventional;
using SystemsRx.Extensions;
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

namespace Game.Systems
{
    public class LevelScreenVisibilitySystem : IManualSystem, IGroupSystem
    {
        public IGroup Group { get; } = new Group(typeof(LevelComponent));
        
        [FromGroup]
        public IObservableGroup ObservableGroup;
        
        private IEventSystem _eventSystem;
        private GameObject _levelImage;
        private LevelComponent _levelComponent;
        private IList<IDisposable> _subscriptions = new List<IDisposable>();

        public LevelScreenVisibilitySystem(IEventSystem eventSystem)
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
                    _levelImage = GameObject.Find("LevelImage");
                    SetupSubscriptions();
                });
        }

        public void SetupSubscriptions()
        {
            _levelComponent.HasLoaded.DistinctUntilChanged(isLoaded => isLoaded)
                .Subscribe(x => _levelImage.SetActive(!_levelComponent.HasLoaded.Value))
                .AddTo(_subscriptions);

            _eventSystem.Receive<PlayerKilledEvent>()
                .Subscribe(x => _levelComponent.HasLoaded.Value = false)
                .AddTo(_subscriptions);
        }

        public void StopSystem()
        { _subscriptions.DisposeAll(); }
    }
}