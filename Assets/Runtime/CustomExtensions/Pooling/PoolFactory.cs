using System;

using Core.Factories;

namespace Core.Pooling
{
    // Here we assume that each spawned object does the work of returning itself to the pool
    // in its own Dispose method
    public class PoolWrapperFactory<T> : IFactory<T>
        where T : class, IDisposable
    {
        private readonly IMemoryPool<T> _pool;

        public PoolWrapperFactory(IMemoryPool<T> pool)
        {
            _pool = pool;
        }

        public T Create()
        {
            return _pool.Spawn();
        }
    }

    public class PoolWrapperFactory<TParam1, TValue> : IFactory<TParam1, TValue>
        where TValue : class, IDisposable
    {
        private readonly IMemoryPool<TParam1, TValue> _pool;

        public PoolWrapperFactory(IMemoryPool<TParam1, TValue> pool)
        {
            _pool = pool;
        }

        public TValue Create(TParam1 arg)
        {
            return _pool.Spawn(arg);
        }
    }
}