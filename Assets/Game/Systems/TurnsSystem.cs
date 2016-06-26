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
        private bool isProcessing;
        private GroupAccessor _levelAccessor;

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
            isProcessing = true;
            yield return new WaitForSeconds(_gameConfiguration.TurnDelay);

            if(!@group.Entities.Any())
            { yield return new WaitForSeconds(_gameConfiguration.TurnDelay); }

            foreach (var enemy in @group.Entities)
            {
                EventSystem.Publish(new EnemyTurnEvent(enemy));
                yield return new WaitForSeconds(_gameConfiguration.MovementTime);
            }

            EventSystem.Publish(new PlayerTurnEvent());
            isProcessing = false;
        }

        public void StartSystem(GroupAccessor @group)
        {
            var waitForLevelToLoad = Observable.EveryUpdate().First(x =>
            {
                var level = _levelAccessor.Entities.First();
                var levelComponent = level.GetComponent<LevelComponent>();
                return levelComponent.HasLoaded.Value;
            });
            
            _updateSubscription = Observable.EveryUpdate().SkipUntil(waitForLevelToLoad).Subscribe(x =>
            {
                if (isProcessing) { return; }
                MainThreadDispatcher.StartCoroutine(CarryOutTurns(@group));
            });
        }

        public void StopSystem(GroupAccessor @group)
        {
            _updateSubscription.Dispose();
        }
    }
}