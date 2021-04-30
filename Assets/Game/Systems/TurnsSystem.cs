using System;
using System.Collections;
using System.Linq;
using SystemsRx.Events;
using SystemsRx.Systems.Conventional;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Configuration;
using Game.Events;
using UniRx;
using UnityEngine;

namespace Game.Systems
{
    public class TurnsSystem : IManualSystem, IGroupSystem
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly IEventSystem _eventSystem;

        private IDisposable _updateSubscription;
        private bool _isProcessing;
        private readonly IObservableGroup _levelAccessor, _enemyAccessor;
        private IEntity _level;

        public IGroup Group { get; } = new Group(typeof(EnemyComponent));

        public TurnsSystem(GameConfiguration gameConfiguration, IEventSystem eventSystem, IObservableGroupManager observableGroupManager)
        {
            _gameConfiguration = gameConfiguration;
            _eventSystem = eventSystem;

            _levelAccessor = observableGroupManager.GetObservableGroup(new Group(typeof (LevelComponent)));
            _enemyAccessor = observableGroupManager.GetObservableGroup(Group);
        }
        
        private IEnumerator CarryOutTurns()
        {
            _isProcessing = true;
            yield return new WaitForSeconds(_gameConfiguration.TurnDelay);

            if(!_enemyAccessor.Any())
            { yield return new WaitForSeconds(_gameConfiguration.TurnDelay); }

            foreach (var enemy in _enemyAccessor)
            {
                _eventSystem.Publish(new EnemyTurnEvent(enemy));
                yield return new WaitForSeconds(_gameConfiguration.MovementTime);
            }

            _eventSystem.Publish(new PlayerTurnEvent());

            _isProcessing = false;
        }

        private bool IsLevelLoaded()
        {
            var levelComponent = _level.GetComponent<LevelComponent>();
            return levelComponent != null && levelComponent.HasLoaded.Value;
        }

        public void StartSystem()
        {
            this.WaitForScene().Subscribe(x => _level = _levelAccessor.First());
            
            _updateSubscription = Observable.EveryUpdate().Where(x => IsLevelLoaded())
                .Subscribe(x => {
                    if (_isProcessing) { return; }
                    MainThreadDispatcher.StartCoroutine(CarryOutTurns());
                });
        }

        public void StopSystem()
        { _updateSubscription.Dispose(); }
    }
}