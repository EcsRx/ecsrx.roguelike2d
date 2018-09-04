using System;
using EcsRx.Collections;
using EcsRx.Extensions;
using EcsRx.Unity.Extensions;
using EcsRx.Views.Components;
using EcsRx.Zenject;
using Game.Blueprints;
using Game.Components;
using Game.Configuration;
using Game.Events;
using Game.Installers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Application : EcsRxApplicationBehaviour
    {
        private IEntityCollection defaultCollection;

        [Inject]
        private GameConfiguration _gameConfiguration;

        protected override void RegisterModules()
        {
            base.RegisterModules();
            DependencyContainer.LoadModule<GameModule>();
            DependencyContainer.LoadModule<SceneCollectionsModule>();
            DependencyContainer.LoadModule<ComputedModule>();
        }

        protected override void ApplicationStarting()
        {
            this.BindAllSystemsWithinApplicationScope();
            this.RegisterAllBoundSystems();
        }

        protected override void ApplicationStarted()
        {
            defaultCollection = CollectionManager.GetCollection();

            var levelBlueprint = new LevelBlueprint();
            var levelEntity = defaultCollection.CreateEntity(levelBlueprint);
            var player = defaultCollection.CreateEntity(new PlayerBlueprint(_gameConfiguration.StartingFoodPoints));
            var playerView = player.GetComponent<ViewComponent>();
            var playerComponent = player.GetComponent<PlayerComponent>();
            var levelComponent = levelEntity.GetComponent<LevelComponent>();

            levelComponent.Level.DistinctUntilChanged()
                .Subscribe(x =>
                {
                    var gameObject = playerView.View as GameObject;
                    gameObject.transform.position = Vector3.zero;
                    SetupLevel(levelComponent);
                });

            EventSystem.Receive<PlayerKilledEvent>()
                .Delay(TimeSpan.FromSeconds(_gameConfiguration.IntroLength))
                .Subscribe(x =>
                {
                    levelBlueprint.UpdateLevel(levelComponent, 1);
                    playerComponent.Food.Value = _gameConfiguration.StartingFoodPoints;
                    SetupLevel(levelComponent);
                });
        }

        private void SetupLevel(LevelComponent levelComponent)
        {
            levelComponent.HasLoaded.Value = false;

            defaultCollection.RemoveEntitiesContaining(typeof(GameBoardComponent),
                typeof(FoodComponent), typeof(WallComponent),
                typeof(EnemyComponent), typeof(ExitComponent));

            Observable.Interval(TimeSpan.FromSeconds(_gameConfiguration.IntroLength))
                .First()
                .Subscribe(x => levelComponent.HasLoaded.Value = true);
            
            defaultCollection.CreateEntity(new GameBoardBlueprint());

            for (var i = 0; i < levelComponent.FoodCount; i++)
            { defaultCollection.CreateEntity(new FoodBlueprint()); }

            for (var i = 0; i < levelComponent.WallCount; i++)
            { defaultCollection.CreateEntity(new WallBlueprint()); }

            for (var i = 0; i < levelComponent.EnemyCount; i++)
            { defaultCollection.CreateEntity(new EnemyBlueprint()); }

            defaultCollection.CreateEntity(new ExitBlueprint());
        }
    }
}
