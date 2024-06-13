using System;
using System.Linq;
using EcsRx.Computeds;
using EcsRx.Groups.Observable;
using EcsRx.Unity.Extensions;
using UniRx;
using UnityEngine;

namespace Game.Computeds
{
    public class ComputedPlayerPosition : ComputedFromGroup<Vector3>, IComputedPlayerPosition
    {
        public ComputedPlayerPosition(IObservableGroup internalObservableGroup) : base(internalObservableGroup)
        {}

        public override IObservable<bool> RefreshWhen()
        { return Observable.EveryUpdate().Select(x => true); }

        public override Vector3 Transform(IObservableGroup observableGroup)
        {
            var player = observableGroup.FirstOrDefault();
            if(player == null)
            { return Vector3.zero; }
            
            var gameObject = player.GetGameObject();
            return gameObject.transform.position;
        }
    }
}