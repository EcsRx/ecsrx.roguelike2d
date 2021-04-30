using System;
using System.Collections.Generic;
using SystemsRx.Events;
using SystemsRx.Extensions;
using SystemsRx.Systems.Conventional;
using EcsRx.Collections;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Extensions;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Plugins.Views.Components;
using Game.Components;
using Game.Events;
using UniRx;
using UniRx.Triggers;

namespace Game.Systems
{
    public class PlayerInteractionSystem : IManualSystem, IGroupSystem
    {
        public IGroup Group { get; } = new Group(typeof (PlayerComponent), typeof (ViewComponent));
        
        private readonly IList<IDisposable> _foodTriggers = new List<IDisposable>();
        private readonly IList<IDisposable> _exitTriggers = new List<IDisposable>();
        private readonly IEventSystem _eventSystem;
        private readonly IObservableGroupManager _observableGroupManager;

        public PlayerInteractionSystem(IEventSystem eventSystem, IObservableGroupManager observableGroupManager)
        {
            _eventSystem = eventSystem;
            _observableGroupManager = observableGroupManager;
        }

        public void StartSystem()
        {
            this.WaitForScene().Subscribe(x =>
            {
                var observableGroup = _observableGroupManager.GetObservableGroup(Group);
                foreach(var player in observableGroup)
                { CheckForInteractions(player); }
            });
        }

        public void StopSystem()
        {
            _foodTriggers.DisposeAll();
            _exitTriggers.DisposeAll();
        }

        private void CheckForInteractions(IEntity player)
        {
            var currentPlayer = player;
            var playerView = currentPlayer.GetGameObject();
            var triggerObservable = playerView.OnTriggerEnter2DAsObservable();
            
            var foodTrigger = triggerObservable
                .Where(x => x.gameObject.CompareTag("Food") || x.gameObject.CompareTag("Soda"))
                .Subscribe(x =>
                {
                    var entityView = x.gameObject.GetComponent<EntityView>();
                    var isSoda = x.gameObject.CompareTag("Soda");
                    HandleFoodPickup(entityView.Entity, currentPlayer, isSoda);
                });

            _foodTriggers.Add(foodTrigger);

            var exitTrigger = triggerObservable
                .Where(x => x.gameObject.CompareTag("Exit"))
                .Subscribe(x =>
                {
                    var entityView = x.gameObject.GetComponent<EntityView>();
                    HandleExit(entityView.Entity, currentPlayer);
                });

            _exitTriggers.Add(exitTrigger);
        }

        private void HandleFoodPickup(IEntity food, IEntity player, bool isSoda)
        { _eventSystem.Publish(new FoodPickupEvent(food, player, isSoda)); }

        private void HandleExit(IEntity exit, IEntity player)
        { _eventSystem.Publish(new ExitReachedEvent(exit, player)); }
    }
}