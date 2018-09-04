#if !NOT_UNITY3D

using System;
using UnityEngine;
namespace Zenject
{
    public class TransformScopeConcreteIdArgNonLazyBinder : ScopeConcreteIdArgNonLazyBinder
    {
        public TransformScopeConcreteIdArgNonLazyBinder(
            BindInfo bindInfo,
            GameObjectCreationParameters gameObjectInfo)
            : base(bindInfo)
        {
            GameObjectInfo = gameObjectInfo;
        }

        protected GameObjectCreationParameters GameObjectInfo
        {
            get;
            private set;
        }

        public ScopeConcreteIdArgNonLazyBinder UnderTransform(Transform parent)
        {
            GameObjectInfo.ParentTransform = parent;
            return this;
        }

        public ScopeConcreteIdArgNonLazyBinder UnderTransform(Func<InjectContext, Transform> parentGetter)
        {
            GameObjectInfo.ParentTransformGetter = parentGetter;
            return this;
        }

        public ScopeConcreteIdArgNonLazyBinder UnderTransformGroup(string transformGroupname)
        {
            GameObjectInfo.GroupName = transformGroupname;
            return this;
        }
    }
}

#endif

