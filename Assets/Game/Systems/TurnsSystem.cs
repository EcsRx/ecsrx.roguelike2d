using System;
using System.Collections;
using System.Linq;
using Assets.Game.Components;
using Assets.Game.Configuration;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Groups;
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

        public IGroup TargetGroup { get { return _targetGroup; } }
        public IEventSystem EventSystem { get; private set; }

        public TurnsSystem(GameConfiguration gameConfiguration, IEventSystem eventSystem)
        {
            _gameConfiguration = gameConfiguration;
            EventSystem = eventSystem;
        }
        
        private IEnumerator CarryOutTurns(GroupAccessor @group)
        {
            yield return new WaitForSeconds(_gameConfiguration.TurnDelay);

            if(!@group.Entities.Any())
            { yield return new WaitForSeconds(_gameConfiguration.TurnDelay); }

            foreach (var enemy in @group.Entities)
            {
                EventSystem.Publish(new EnemyTurnEvent(enemy));
                yield return new WaitForSeconds(_gameConfiguration.MovementTime);
            }

            EventSystem.Publish(new PlayerTurnEvent());
        }

        public void StartSystem(GroupAccessor @group)
        {
            _updateSubscription = Observable.EveryUpdate().Subscribe(x =>
            {
                MainThreadDispatcher.StartUpdateMicroCoroutine(CarryOutTurns(@group));
            });
        }

        public void StopSystem(GroupAccessor @group)
        {
            _updateSubscription.Dispose();
        }
    }
}