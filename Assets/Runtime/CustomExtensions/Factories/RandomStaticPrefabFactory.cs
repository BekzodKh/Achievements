using UnityEngine;

using VContainer;
using VContainer.Unity;

using Core.Utils.IndexGenerators;

namespace Core.Factories
{
    public class RandomStaticPrefabFactory<T> : IFactory<T>
        where T : Component
    {
        private readonly T[] _prefabs;
        private readonly IObjectResolver _resolver;
        private readonly Transform _parent;
        private readonly IIndexGenerator _indexGenerator;

        protected RandomStaticPrefabFactory(T[] prefabs, IObjectResolver resolver, Transform parent,
            IIndexGenerator indexGenerator)
        {
            _prefabs = prefabs;
            _resolver = resolver;
            _parent = parent;
            _indexGenerator = indexGenerator;
        }

        public T Create()
        {
            var prefab = _prefabs[_indexGenerator.Get()];

            return _resolver.Instantiate(prefab, _parent);
        }
    }
}