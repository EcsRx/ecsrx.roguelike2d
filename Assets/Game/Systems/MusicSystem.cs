using System;
using System.Collections.Generic;
using System.Linq;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UniRx;
using UnityEngine;

namespace Game.Systems
{
    public class MusicSystem : IManualSystem
    {
        public IGroup Group { get; } = new Group(typeof(LevelComponent));

        private readonly IEventSystem _eventSystem;
        private readonly AudioSource _musicSource;
        private LevelComponent _levelComponent;
        private readonly IList<IDisposable> _subscriptions = new List<IDisposable>();

        public MusicSystem(IEventSystem eventSystem)
        {
            var soundEffectObject = GameObject.Find("MusicSource");
            _musicSource = soundEffectObject.GetComponent<AudioSource>();
            _eventSystem = eventSystem;
        }

        public void StartSystem(IObservableGroup group)
        {
            this.WaitForScene()
                .Subscribe(x =>
                {
                    var level = group.First();
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

        public void StopSystem(IObservableGroup group)
        { _subscriptions.DisposeAll(); }
    }
    
}