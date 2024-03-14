using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace Core.Factories
{
    public class StaticPrefabFactory<T> : IFactory<T>
        where T : Component
    {
        private readonly T _prefab;
        private readonly IObjectResolver _objectResolver;
        private readonly Transform _parent;

        public StaticPrefabFactory(T prefab, IObjectResolver objectResolver, Transform parent)
        {
            _objectResolver = objectResolver;
            _parent = parent;
            _prefab = prefab;
        }

        public virtual T Create()
        {
            return _objectResolver.Instantiate(_prefab, _parent);
        }
    }
}
