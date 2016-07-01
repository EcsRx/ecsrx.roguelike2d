using System;
using System.Collections;
using System.Linq;
using Assets.Game.Components;
using Assets.Game.Configuration;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Pools;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class TurnsSystem : IManualSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(EnemyComponent));
        private readonly GameConfiguration _gameConfiguration;
        private readonly IEventSystem _eventSystem;

        private IDisposable _updateSubscription;
        private bool _isProcessing;
        private readonly GroupAccessor _levelAccessor;
        private IEntity _level;

        public IGroup TargetGroup { get { return _targetGroup; } }

        public TurnsSystem(GameConfiguration gameConfiguration, IEventSystem eventSystem, IPoolManager poolManager)
        {
            _gameConfiguration = gameConfiguration;
            _eventSystem = eventSystem;

            _levelAccessor = poolManager.CreateGroupAccessor(new Group(typeof (LevelComponent)));
        }
        
        private IEnumerator CarryOutTurns(GroupAccessor @group)
        {
            _isProcessing = true;
            yield return new WaitForSeconds(_gameConfiguration.TurnDelay);

            if(!@group.Entities.Any())
            { yield return new WaitForSeconds(_gameConfiguration.TurnDelay); }

            var enemies = @group.Entities.ToArray();
            foreach (var enemy in enemies)
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

        public void StartSystem(GroupAccessor @group)
        {
            this.WaitForScene().Subscribe(x => _level = _levelAccessor.Entities.First());
            
            _updateSubscription = Observable.EveryUpdate().Where(x => IsLevelLoaded())
                .Subscribe(x => {
                    if (_isProcessing) { return; }
                    MainThreadDispatcher.StartCoroutine(CarryOutTurns(@group));
                });
        }

        public void StopSystem(GroupAccessor @group)
        {
            _updateSubscription.Dispose();
        }
    }
}