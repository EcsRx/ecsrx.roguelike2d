using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Systems
{
    public class FoodTextUpdateSystem : IManualSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(PlayerComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        private IEventSystem _eventSystem;
        private PlayerComponent _playerComponent;
        private Text _foodText;
        private IList<IDisposable> _subscriptions = new List<IDisposable>();

        public FoodTextUpdateSystem(IEventSystem eventSystem)
        { _eventSystem = eventSystem; }

        public void StartSystem(GroupAccessor @group)
        {
            this.WaitForScene().Subscribe(x =>
            {
                var player = @group.Entities.First();
                _playerComponent = player.GetComponent<PlayerComponent>();
                _foodText = GameObject.Find("FoodText").GetComponent<Text>();

                SetupSubscriptions();
            });
        }

        private void SetupSubscriptions()
        {
            _playerComponent.Food.DistinctUntilChanged()
                .Subscribe(foodAmount => { _foodText.text = string.Format("Food: {0}", foodAmount); })
                .AddTo(_subscriptions);

            _eventSystem.Receive<FoodPickupEvent>()
                .Subscribe(x =>
                {
                    var foodPoints = x.Food.GetComponent<FoodComponent>().FoodAmount;
                    _foodText.text = string.Format("+{0} Food: {1}", foodPoints, _playerComponent.Food.Value);
                })
                .AddTo(_subscriptions);

            _eventSystem.Receive<PlayerHitEvent>()
                .Subscribe(x =>
                {
                    var attackScore = x.Enemy.GetComponent<EnemyComponent>().EnemyPower;
                    _foodText.text = string.Format("-{0} Food: {1}", attackScore, _playerComponent.Food.Value);
                })
                .AddTo(_subscriptions);
        }

        public void StopSystem(GroupAccessor @group)
        {
            _subscriptions.DisposeAll();
        }
    }
}