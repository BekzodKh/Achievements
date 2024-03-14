using UnityEngine;

using VContainer;

using Core.Pooling;

namespace Core.Factories
{
    public class MonoMemoryPoolFactory<TComponent>: IFactory<TComponent, MonoMemoryPool<TComponent>>
        where TComponent : Component
    {
        private readonly IObjectResolver _resolver;
        private readonly MemoryPoolSettings _settings;
        private readonly Transform _parent;

        protected MonoMemoryPoolFactory(IObjectResolver resolver, MemoryPoolSettings settings, Transform parent)
        {
            _resolver = resolver;
            _settings = settings;
            _parent = parent;
        }

        public MonoMemoryPool<TComponent> Create(TComponent prefab)
        {
            var factory = new StaticPrefabFactory<TComponent>(prefab, _resolver, _parent);

            var pool = new MonoMemoryPool<TComponent>();
            pool.Construct(factory, _settings);
            pool.Initialize();

            return pool;
        }
    }
}