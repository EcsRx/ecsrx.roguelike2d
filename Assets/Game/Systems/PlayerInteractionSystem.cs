using System;
using System.Collections.Generic;
using Assets.Game.Components;
using Assets.Game.Events;
using EcsRx.Entities;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using EcsRx.Unity.MonoBehaviours;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Game.Systems
{
    public class PlayerInteractionSystem : IManualSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof (PlayerComponent), typeof (ViewComponent));

        public IGroup TargetGroup
        {
            get { return _targetGroup; }
        }

        private readonly IList<IDisposable> _foodTriggers = new List<IDisposable>();
        private readonly IList<IDisposable> _exitTriggers = new List<IDisposable>();
        private readonly IEventSystem _eventSystem;

        public PlayerInteractionSystem(IEventSystem eventSystem)
        {
            _eventSystem = eventSystem;
        }

        public void StartSystem(GroupAccessor @group)
        {
            this.WaitForScene().Subscribe(x =>
            {
                foreach(var player in @group.Entities)
                { CheckForInteractions(player); }
            });
        }

        public void StopSystem(GroupAccessor @group)
        {
            _foodTriggers.DisposeAll();
            _exitTriggers.DisposeAll();
        }

        private void CheckForInteractions(IEntity player)
        {
            var currentPlayer = player;
            var playerView = currentPlayer.GetComponent<ViewComponent>().View;
            var triggerObservable = playerView.OnTriggerEnter2DAsObservable();
            
            var foodTrigger = triggerObservable
                .Where(x => x.gameObject.tag == "Food" || x.gameObject.tag == "Soda")
                .Subscribe(x =>
                {
                    var entityView = x.gameObject.GetComponent<EntityView>();
                    var isSoda = x.gameObject.tag == "Soda";
                    HandleFoodPickup(entityView.Entity, currentPlayer, isSoda);
                });

            _foodTriggers.Add(foodTrigger);

            var exitTrigger = triggerObservable
                .Where(x => x.gameObject.tag == "Exit")
                .Subscribe(x =>
                {
                    var entityView = x.gameObject.GetComponent<EntityView>();
                    HandleExit(entityView.Entity, currentPlayer);
                });

            _exitTriggers.Add(exitTrigger);
        }

        private void HandleFoodPickup(IEntity food, IEntity player, bool isSoda)
        {
            _eventSystem.Publish(new FoodPickupEvent(food, player, isSoda));
        }

        private void HandleExit(IEntity exit, IEntity player)
        {
            _eventSystem.Publish(new ExitReachedEvent(exit, player));
        }
    }
}