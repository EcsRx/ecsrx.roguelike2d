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
using EcsRx.Plugins.GroupBinding.Attributes;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Configuration;
using Game.Events;
using UniRx;
using UnityEngine;

namespace Game.Systems
{
    public class TurnsSystem : IManualSystem
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly IEventSystem _eventSystem;

        private IDisposable _updateSubscription;
        private bool _isProcessing;
        
        [FromComponents(typeof (LevelComponent))]
        public IObservableGroup LevelAccessor;
        
        [FromComponents(typeof(EnemyComponent))]
        public IObservableGroup EnemyAccessor;
        
        private IEntity _level;

        public TurnsSystem(GameConfiguration gameConfiguration, IEventSystem eventSystem)
        {
            _gameConfiguration = gameConfiguration;
            _eventSystem = eventSystem;
        }
        
        private IEnumerator CarryOutTurns()
        {
            _isProcessing = true;
            yield return new WaitForSeconds(_gameConfiguration.TurnDelay);

            if(!EnemyAccessor.Any())
            { yield return new WaitForSeconds(_gameConfiguration.TurnDelay); }

            foreach (var enemy in EnemyAccessor)
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
            this.WaitForScene().Subscribe(x => _level = LevelAccessor.First());
            
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