
using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Zenject
{
    // When the app starts up, typically there is a list of instances that need to be injected
    // The question is, what is the order that they should be injected?  Originally we would
    // just iterate over the list and inject in whatever order they were in
    // What is better than that though, is to inject based on their dependency order
    // So if A depends on B then it would be nice if B was always injected before A
    // That way, in [Inject] methods for A, A can access members on B knowing that it's
    // already been initialized.
    // So in order to do this, we add the initial pool of instances to this class then
    // notify this class whenever an instance is resolved via a FromInstance binding
    // That way we can lazily call inject on-demand whenever the instance is requested
    public class LazyInstanceInjector
    {
        readonly DiContainer _container;
        readonly HashSet<object> _instancesToInject = new HashSet<object>();

        public LazyInstanceInjector(DiContainer container)
        {
            _container = container;
        }

        public IEnumerable<object> Instances
        {
            get { return _instancesToInject; }
        }

        public void AddInstance(object instance)
        {
            _instancesToInject.Add(instance);
        }

        public void AddInstances(IEnumerable<object> instances)
        {
            _instancesToInject.UnionWith(instances);
        }

        public void LazyInject(object instance)
        {
            if (_instancesToInject.Remove(instance))
            {
                _container.Inject(instance);
            }
        }

        public void LazyInjectAll()
        {
#if UNITY_EDITOR
            using (ProfileBlock.Start("LazyInstanceInjector.LazyInjectAll"))
#endif
            {
                var tempList = new List<object>();
                while (!_instancesToInject.IsEmpty())
                {
                    tempList.Clear();
                    tempList.AddRange(_instancesToInject);
                    _instancesToInject.Clear();
                    foreach (var instance in tempList)
                    {
                        _container.Inject(instance);
                    }
                }
            }
        }
    }
}

