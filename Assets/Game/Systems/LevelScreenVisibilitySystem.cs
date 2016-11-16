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

namespace Assets.Game.Systems
{
    public class LevelScreenVisibilitySystem : IManualSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(LevelComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        private GameObject _levelImage;
        private LevelComponent _levelComponent;
        private IEventSystem _eventSystem;
        private IList<IDisposable> _subscriptions = new List<IDisposable>();

        public LevelScreenVisibilitySystem(IEventSystem eventSystem)
        { _eventSystem = eventSystem; }

        public void StartSystem(IGroupAccessor @group)
        {
            this.WaitForScene()
                .Subscribe(x =>
                {
                    var level = @group.Entities.First();
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

        public void StopSystem(IGroupAccessor @group)
        {
            _subscriptions.DisposeAll();
        }
    }
}