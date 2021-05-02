using System;
using System.Collections.Generic;
using System.Linq;
using SystemsRx.Attributes;
using SystemsRx.Events;
using SystemsRx.Extensions;
using SystemsRx.Systems.Conventional;
using EcsRx.Collections;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Plugins.GroupBinding.Attributes;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using Game.Components;
using Game.Events;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Systems
{
    [Priority(10)]
    public class FoodTextUpdateSystem : IManualSystem, IGroupSystem
    {
        public IGroup Group { get; } = new Group(typeof(PlayerComponent));

        [FromGroup]
        public IObservableGroup ObservableGroup;
        
        private readonly IEventSystem _eventSystem;
        private PlayerComponent _playerComponent;
        private Text _foodText;
        private readonly IList<IDisposable> _subscriptions = new List<IDisposable>();

        public FoodTextUpdateSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public void StartSystem()
        {
            this.WaitForScene().Subscribe(x =>
            {
                var player = ObservableGroup.First();
                _playerComponent = player.GetComponent<PlayerComponent>();
                _foodText = GameObject.Find("FoodText").GetComponent<Text>();
                SetupSubscriptions();
            });
        }

        private void SetupSubscriptions()
        {
            _playerComponent.Food.DistinctUntilChanged()
                .Subscribe(foodAmount => { _foodText.text = $"Food: {foodAmount}"; })
                .AddTo(_subscriptions);

            _eventSystem.Receive<FoodPickupEvent>()
                .Subscribe(x =>
                {
                    var foodComponent = x.Food.GetComponent<FoodComponent>();
                    var foodPoints = foodComponent.FoodAmount;
                    _foodText.text = $"+{foodPoints} Food: {_playerComponent.Food.Value}";
                })
                .AddTo(_subscriptions);

            _eventSystem.Receive<PlayerHitEvent>()
                .Subscribe(x =>
                {
                    var attackScore = x.Enemy.GetComponent<EnemyComponent>().EnemyPower;
                    _foodText.text = $"-{attackScore} Food: {_playerComponent.Food.Value}";
                })
                .AddTo(_subscriptions);
        }

        public void StopSystem()
        { _subscriptions.DisposeAll(); }
    }
}