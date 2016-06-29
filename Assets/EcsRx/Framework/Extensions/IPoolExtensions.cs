using System;
using System.Linq;
using EcsRx.Components;
using EcsRx.Pools;

namespace EcsRx.Extensions
{
    public static class IPoolExtensions
    {
        public static void RemoveEntitiesContaining<T>(this IPool pool)
            where T : class, IComponent
        {
            pool.Entities.Where(entity => entity.HasComponent<T>())
                .ToList()
                .ForEachRun(pool.RemoveEntity);
        }

        public static void RemoveEntitiesContaining(this IPool pool, params Type[] components)
        {
            pool.Entities.Where(entity => components.Any(x => entity.HasComponents(x)))
                .ToList()
                .ForEachRun(pool.RemoveEntity);
        }
    }
}