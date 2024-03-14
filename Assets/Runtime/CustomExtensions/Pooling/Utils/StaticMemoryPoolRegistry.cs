using System;
using System.Collections.Generic;

namespace Core.Pooling.Utils
{
#if UNITY_EDITOR
    public static class StaticMemoryPoolRegistry
    {
        public static event Action<IMemoryPool> PoolAdded = delegate {};
        public static event Action<IMemoryPool> PoolRemoved = delegate {};

        private static readonly List<IMemoryPool> _pools = new List<IMemoryPool>();

        public static IEnumerable<IMemoryPool> Pools => _pools;

        public static void Add(IMemoryPool memoryPool)
        {
            _pools.Add(memoryPool);
            PoolAdded(memoryPool);
        }

        public static void Remove(IMemoryPool memoryPool)
        {
            _pools.Remove(memoryPool);

            PoolRemoved(memoryPool);
        }
    }
#endif
}
