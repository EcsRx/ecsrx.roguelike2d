#if !NOT_UNITY3D

using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;

namespace Zenject
{
    public class PrefabInstantiator : IPrefabInstantiator
    {
        readonly IPrefabProvider _prefabProvider;
        readonly DiContainer _container;
        readonly List<TypeValuePair> _extraArguments;
        readonly GameObjectCreationParameters _gameObjectBindInfo;
        readonly Type _argumentTarget;

        public PrefabInstantiator(
            DiContainer container,
            GameObjectCreationParameters gameObjectBindInfo,
            Type argumentTarget,
            List<TypeValuePair> extraArguments,
            IPrefabProvider prefabProvider)
        {
            _prefabProvider = prefabProvider;
            _extraArguments = extraArguments;
            _container = container;
            _gameObjectBindInfo = gameObjectBindInfo;
            _argumentTarget = argumentTarget;
        }

        public GameObjectCreationParameters GameObjectCreationParameters
        {
            get { return _gameObjectBindInfo; }
        }

        public Type ArgumentTarget
        {
            get { return _argumentTarget; }
        }

        public List<TypeValuePair> ExtraArguments
        {
            get { return _extraArguments; }
        }

        public UnityEngine.Object GetPrefab()
        {
            return _prefabProvider.GetPrefab();
        }

        public GameObject Instantiate(List<TypeValuePair> args, out Action injectAction)
        {
            var context = new InjectContext(_container, _argumentTarget, null);
            bool shouldMakeActive;
            var gameObject = _container.CreateAndParentPrefab(
                GetPrefab(), _gameObjectBindInfo, context, out shouldMakeActive);
            Assert.IsNotNull(gameObject);

            injectAction = () =>
            {
                var allArgs = _extraArguments.Concat(args).ToList();

                if (_argumentTarget == null)
                {
                    Assert.That(
                        allArgs.IsEmpty(),
                        "Unexpected arguments provided to prefab instantiator.  Arguments are not allowed if binding multiple components in the same binding");
                }

                if (_argumentTarget == null || allArgs.IsEmpty())
                {
                    _container.InjectGameObject(gameObject);
                }
                else
                {
                    var injectArgs = new InjectArgs()
                    {
                        ExtraArgs = allArgs,
                        Context = context,
                        ConcreteIdentifier = null
                    };

                    _container.InjectGameObjectForComponentExplicit(
                        gameObject, _argumentTarget, injectArgs);
                }

                if (shouldMakeActive)
                {
                    gameObject.SetActive(true);
                }
            };

            return gameObject;
        }
    }
}

#endif
