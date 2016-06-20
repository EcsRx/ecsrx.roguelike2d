using System;
using System.Collections.Generic;
using Assets.Game.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Unity.Components;

namespace Assets.Game.Groups
{
    public class EnemyGroup : IGroup
    {
        public IEnumerable<Type> TargettedComponents
        {
            get
            {
                return new[] { typeof(ViewComponent), typeof(EnemyComponent), typeof(RandomlyPlacedComponent) };
            }
        }

        public Predicate<IEntity> TargettedEntities
        {
            get { return null; }
        }
    }
}