using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace Core.Factories
{
    public class RuntimePrefabFactory<T> : IFactory<T, T>
        where T : Component
    {
        private readonly IObjectResolver _objectResolver;

        protected RuntimePrefabFactory(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public virtual T Create(T prefab)
        {
            return _objectResolver.Instantiate(prefab);
        }
    }
}
