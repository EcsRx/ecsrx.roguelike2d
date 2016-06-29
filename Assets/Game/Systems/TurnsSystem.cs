using System;
using System.Collections;
using System.Linq;
using Assets.Game.Components;
using Assets.Game.Configuration;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
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
        private IDisposable _updateSubscription;
        private bool _isProcessing;
        private readonly GroupAccessor _levelAccessor;
        private LevelComponent _levelComponent;

        public IGroup TargetGroup { get { return _targetGroup; } }
        public IEventSystem EventSystem { get; private set; }

        public TurnsSystem(GameConfiguration gameConfiguration, IEventSystem eventSystem, IPoolManager poolManager)
        {
            _gameConfiguration = gameConfiguration;
            EventSystem = eventSystem;

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
                //EventSystem.Publish(new EnemyTurnEvent(enemy));
                yield return new WaitForSeconds(_gameConfiguration.MovementTime);
            }

            EventSystem.Publish(new PlayerTurnEvent());
            _isProcessing = false;
        }

        private bool IsLevelLoaded()
        {
            return _levelComponent != null && _levelComponent.HasLoaded.Value;
        }

        public void StartSystem(GroupAccessor @group)
        {
            Observable.EveryUpdate().First()
                .Subscribe(x => {
                    var level = _levelAccessor.Entities.First();
                    _levelComponent = level.GetComponent<LevelComponent>();
                });
            
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