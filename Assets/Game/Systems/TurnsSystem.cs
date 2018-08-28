using System;
using System.Collections;
using System.Linq;
using Assets.Game.Components;
using Assets.Game.Configuration;
using Assets.Game.Events;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class TurnsSystem : IManualSystem
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly IEventSystem _eventSystem;

        private IDisposable _updateSubscription;
        private bool _isProcessing;
        private readonly IObservableGroup _levelAccessor;
        private IEntity _level;

        public IGroup Group { get; } = new Group(typeof(EnemyComponent));

        public TurnsSystem(GameConfiguration gameConfiguration, IEventSystem eventSystem, IEntityCollectionManager entityCollectionManager)
        {
            _gameConfiguration = gameConfiguration;
            _eventSystem = eventSystem;

            _levelAccessor = entityCollectionManager.GetObservableGroup(new Group(typeof (LevelComponent)));
        }
        
        private IEnumerator CarryOutTurns(IObservableGroup group)
        {
            _isProcessing = true;
            yield return new WaitForSeconds(_gameConfiguration.TurnDelay);

            if(!group.Any())
            { yield return new WaitForSeconds(_gameConfiguration.TurnDelay); }

            foreach (var enemy in group)
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

        public void StartSystem(IObservableGroup group)
        {
            this.WaitForScene().Subscribe(x => _level = _levelAccessor.First());
            
            _updateSubscription = Observable.EveryUpdate().Where(x => IsLevelLoaded())
                .Subscribe(x => {
                    if (_isProcessing) { return; }
                    MainThreadDispatcher.StartCoroutine(CarryOutTurns(@group));
                });
        }

        public void StopSystem(IObservableGroup group)
        { _updateSubscription.Dispose(); }
    }
}