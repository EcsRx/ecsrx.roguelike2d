using Assets.Game.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Systems
{
    public class FoodTextUpdateSystem : IReactToEntitySystem
    {
        private readonly IGroup _targetGroup = new Group(typeof(PlayerComponent));

        private readonly Text _foodText;

        public IGroup TargetGroup { get { return _targetGroup; } }

        public FoodTextUpdateSystem()
        {
            _foodText = GameObject.Find("FoodText").GetComponent<Text>();
        }

        public IObservable<IEntity> ReactToEntity(IEntity entity)
        {
            var playerComponent = entity.GetComponent<PlayerComponent>();
            return playerComponent.Food.DistinctUntilChanged().Select(x => entity);
        }

        public void Execute(IEntity entity)
        {
            var levelComponent = entity.GetComponent<PlayerComponent>();
            _foodText.text = string.Format("Food: {0}", levelComponent.Food.Value);
        }
    }
}