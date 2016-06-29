using System.Linq;
using Assets.Game.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;

namespace Assets.Game.Systems
{
    /*
    public class MusicSystem : IManualSystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(PlayerComponent));
        public IGroup TargetGroup { get { return _targetGroup; } }

        private AudioSource _musicSource;
        private PlayerComponent _playerComponent;

        public MusicSystem()
        {
            var soundEffectObject = GameObject.Find("MusicSource");
            _musicSource = soundEffectObject.GetComponent<AudioSource>();
        }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            return 
        }

        public void Execute(IEntity entity)
        {
            _musicSource.Stop();
        }

        public bool IsPlayerAlive()
        {
            return _playerComponent != null && _playerComponent.Food.Value > 0;
        }

        public void StartSystem(GroupAccessor @group)
        {
            Observable.EveryUpdate().First()
                .Subscribe(x => {
                    var player = @group.Entities.First();
                    _playerComponent = player.GetComponent<PlayerComponent>();
                });



            var s = entity.GetComponent<PlayerComponent>().Food.Where(x => x <= 0).Select(x => entity);
        }

        private void StopPlaying

        public void StopSystem(GroupAccessor @group)
        {
            throw new System.NotImplementedException();
        }
    }
    */
}