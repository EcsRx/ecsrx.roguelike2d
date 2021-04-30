using System;
using System.Collections.Generic;
using System.Linq;
using SystemsRx.Events;
using SystemsRx.Systems.Conventional;
using SystemsRx.Extensions;
using EcsRx.Collections;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UniRx;
using UnityEngine;

namespace Game.Systems
{
    public class MusicSystem : IManualSystem, IGroupSystem
    {
        public IGroup Group { get; } = new Group(typeof(LevelComponent));

        private readonly IEventSystem _eventSystem;
        private readonly IObservableGroupManager _observableGroupManager;
        
        private readonly AudioSource _musicSource;
        private LevelComponent _levelComponent;
        private readonly IList<IDisposable> _subscriptions = new List<IDisposable>();

        public MusicSystem(IEventSystem eventSystem, IObservableGroupManager observableGroupManager)
        {
            var soundEffectObject = GameObject.Find("MusicSource");
            _musicSource = soundEffectObject.GetComponent<AudioSource>();
            _eventSystem = eventSystem;
            _observableGroupManager = observableGroupManager;
        }

        public void StartSystem()
        {
            this.WaitForScene()
                .Subscribe(x =>
                {
                    var level = _observableGroupManager.GetObservableGroup(Group).First();
                    _levelComponent = level.GetComponent<LevelComponent>();
                    SetupSubscriptions();
                });
        }

        private void SetupSubscriptions()
        {
            _eventSystem.Receive<PlayerKilledEvent>()
                .Subscribe(x => _musicSource.Stop())
                .AddTo(_subscriptions);

            _levelComponent.HasLoaded
                .DistinctUntilChanged(hasLoaded => hasLoaded)
                .Subscribe(x =>
                {
                    if(!_musicSource.isPlaying)
                    { _musicSource.Play(); }
                })
                .AddTo(_subscriptions);
        }

        public void StopSystem()
        { _subscriptions.DisposeAll(); }
    }
    
}