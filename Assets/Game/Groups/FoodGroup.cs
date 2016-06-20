using System;
using System.Collections.Generic;
using Assets.Game.Components;
using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Unity.Components;

namespace Assets.Game.Groups
{
    public class FoodGroup : IGroup
    {
        public IEnumerable<Type> TargettedComponents
        {
            get
            {
                return new[] { typeof(ViewComponent), typeof(FoodComponent), typeof(RandomlyPlacedComponent) };
            }
        }

        public Predicate<IEntity> TargettedEntities
        {
            get { return null; }
        }
    }
}