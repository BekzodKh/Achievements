using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

namespace Core.Pooling.Utils
{
    // If you want to ensure that all items are always returned to the pool, include the following
    // in an installer
    // Container.BindInterfacesTo<PoolCleanupChecker>().AsSingle()
    public class PoolCleanupChecker : IDisposable
    {
        private readonly List<IMemoryPool> _poolFactories;

        public PoolCleanupChecker(List<IMemoryPool> poolFactories)
        {
            _poolFactories = poolFactories;
        }

        void IDisposable.Dispose()
        {
            foreach (var pool in _poolFactories)
            {
                Assert.AreEqual(pool.NumActive, 0, $"Found active objects in pool '{pool.GetType()}' during dispose.  Did you forget to despawn an object of type '{pool.ItemType}'?");
            }
        }
    }
}
