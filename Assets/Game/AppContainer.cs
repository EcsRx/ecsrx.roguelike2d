using System;
using Assets.Game.Blueprints;
using Assets.Game.Components;
using Assets.Game.Configuration;
using EcsRx.Pools;
using EcsRx.Unity;
using UniRx;
using Zenject;

public class AppContainer : EcsRxContainer
{
    private IPool defaultPool;

    [Inject]
    private GameConfiguration _gameConfiguration;

    protected override void SetupSystems()
    {}

    protected override void SetupEntities()
    {
        defaultPool = PoolManager.GetPool();

        SetupLevel();
    }

    private void SetupLevel()
    {
        var levelEntity = defaultPool.CreateEntity(new LevelBlueprint());
        var levelComponent = levelEntity.GetComponent<LevelComponent>();
        
        Observable.Interval(TimeSpan.FromSeconds(_gameConfiguration.IntroLength))
            .First()
            .Subscribe(x => levelComponent.HasLoaded.Value = true);
            
        defaultPool.CreateEntity(new GameBoardBlueprint());

        for (var i = 0; i < levelComponent.FoodCount; i++)
        { defaultPool.CreateEntity(new FoodBlueprint()); }

        for (var i = 0; i < levelComponent.WallCount; i++)
        { defaultPool.CreateEntity(new WallBlueprint()); }

        for (var i = 0; i < levelComponent.EnemyCount; i++)
        { defaultPool.CreateEntity(new EnemyBlueprint()); }

        defaultPool.CreateEntity(new ExitBlueprint());
        defaultPool.CreateEntity(new PlayerBlueprint(_gameConfiguration.StartingFoodPoints));
    }
}
