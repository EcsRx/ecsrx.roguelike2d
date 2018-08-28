﻿using System;
using System.Collections.Generic;
using EcsRx.Events;
using EcsRx.Extensions;
using EcsRx.Groups;
using EcsRx.Groups.Observable;
using EcsRx.Systems;
using Game.Events;
using Game.Extensions;
using Game.SceneCollections;
using UniRx;
using UnityEngine;

namespace Game.Systems
{
    public class SoundEffectSystem : IManualSystem
    {
        public IGroup Group { get; } = new EmptyGroup();

        private readonly AudioSource _soundEffectSource;
        private readonly EnemyAttackSounds _enemyAttackSounds;
        private readonly PlayerAttackSounds _playerAttackSounds;
        private readonly WalkingSounds _walkingSounds;
        private readonly DeathSounds _deathSounds;
        private readonly DrinkSounds _drinkSounds;
        private readonly FoodSounds _foodSounds;

        private readonly IEventSystem _eventSystem;
        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();

        public SoundEffectSystem(EnemyAttackSounds enemyAttackSounds, PlayerAttackSounds playerAttackSounds, 
            WalkingSounds walkingSounds, DeathSounds deathSounds, DrinkSounds drinkSounds, 
            FoodSounds foodSounds, IEventSystem eventSystem)
        {
            _enemyAttackSounds = enemyAttackSounds;
            _playerAttackSounds = playerAttackSounds;
            _walkingSounds = walkingSounds;
            _deathSounds = deathSounds;
            _drinkSounds = drinkSounds;
            _foodSounds = foodSounds;

            _eventSystem = eventSystem;

            var soundEffectObject = GameObject.Find("SoundEffectSource");
            _soundEffectSource = soundEffectObject.GetComponent<AudioSource>();
        }

        public void StartSystem(IObservableGroup group)
        {
            _eventSystem.Receive<FoodPickupEvent>().Subscribe(x => {
                var clips = x.IsSoda ? _drinkSounds.AvailableClips : _foodSounds.AvailableClips;
                PlayOneOf(clips);
            }).AddTo(_subscriptions);

            _eventSystem.Receive<EntityMovedEvent>()
                .Subscribe(x => PlayOneOf(_walkingSounds.AvailableClips))
                .AddTo(_subscriptions);

            _eventSystem.Receive<EnemyHitEvent>()
                .Subscribe(x => PlayOneOf(_playerAttackSounds.AvailableClips))
                .AddTo(_subscriptions);

            _eventSystem.Receive<WallHitEvent>()
                .Subscribe(x => PlayOneOf(_playerAttackSounds.AvailableClips))
                .AddTo(_subscriptions);

            _eventSystem.Receive<PlayerHitEvent>()
                .Subscribe(x => PlayOneOf(_enemyAttackSounds.AvailableClips))
                .AddTo(_subscriptions);

            _eventSystem.Receive<PlayerKilledEvent>()
                .Subscribe(x => PlayOneOf(_deathSounds.AvailableClips))
                .AddTo(_subscriptions);
        }

        private void PlayOneOf(IEnumerable<AudioClip> clips)
        {
            var audioSource = clips.TakeRandom();
            _soundEffectSource.PlayOneShot(audioSource);
        }

        public void StopSystem(IObservableGroup group)
        { _subscriptions.DisposeAll(); }
    }
}