using System.Collections.Generic;

using JetBrains.Annotations;

namespace Core.Pooling.Utils
{
    public class ArrayPool<T> : StaticMemoryPoolBaseBase<T[]>
    {
        private readonly int _length;

        private ArrayPool(int length)
            : base(OnDespawned)
        {
            _length = length;
        }

        private static void OnDespawned(T[] arr)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = default;
            }
        }

        [PublicAPI]
        public T[] Spawn()
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                return SpawnInternal();
            }
        }

        protected override T[] Alloc()
        {
            return new T[_length];
        }

        private static readonly Dictionary<int, ArrayPool<T>> _pools = new Dictionary<int, ArrayPool<T>>();

        [PublicAPI]
        public static ArrayPool<T> GetPool(int length)
        {
            if (_pools.TryGetValue(length, out var pool)) return pool;

            pool = new ArrayPool<T>(length);
            _pools.Add(length, pool);

            return pool;
        }
    }
}
