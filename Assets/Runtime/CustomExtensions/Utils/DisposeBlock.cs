using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

using JetBrains.Annotations;

using Core.Pooling;
using Core.Pooling.Utils;

namespace Core.Utils
{
    public class DisposeBlock : IDisposable
    {
        private static readonly StaticMemoryPool<DisposeBlock> _pool =
            new StaticMemoryPool<DisposeBlock>(OnSpawned, OnDespawned);

        private List<IDisposable> _disposables;
        private List<SpawnedObjectPoolPair> _objectPoolPairs;

        [Obsolete("Use DisposeBlock.Spawn() for pooling")]
        public DisposeBlock() { }

#if UNITY_EDITOR
        // Required for disabling domain reload in enter the play mode feature. See: https://docs.unity3d.com/Manual/DomainReloading.html
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticValues()
        {
            if (!UnityEditor.EditorSettings.enterPlayModeOptionsEnabled)
            {
                return;
            }

            _pool.Clear();
        }
#endif

        private static void OnSpawned(DisposeBlock that)
        {
            Assert.IsNull(that._disposables);
            Assert.IsNull(that._objectPoolPairs);
        }

        private static void OnDespawned(DisposeBlock that)
        {
            if (that._disposables != null)
            {
                // Dispose in reverse order since usually that makes the most sense
                for (var i = that._disposables.Count - 1; i >= 0; i--)
                {
                    that._disposables[i].Dispose();
                }

                ListPool<IDisposable>.Instance.Despawn(that._disposables);
                that._disposables = null;
            }

            if (that._objectPoolPairs == null) return;

            // Dispose in reverse order since usually that makes the most sense
            for (var i = that._objectPoolPairs.Count - 1; i >= 0; i--)
            {
                var pair = that._objectPoolPairs[i];
                pair.Pool.Despawn(pair.Object);
            }

            ListPool<SpawnedObjectPoolPair>.Instance.Despawn(that._objectPoolPairs);
            that._objectPoolPairs = null;
        }

        private void LazyInitializeDisposableList()
        {
            if (_disposables == null)
            {
                _disposables = ListPool<IDisposable>.Instance.Spawn();
            }
        }

        [PublicAPI]
        public void AddRange<T>(IEnumerable<T> disposables)
            where T : IDisposable
        {
            LazyInitializeDisposableList();

            foreach (var disposable in disposables)
            {
                _disposables.Add(disposable);
            }
        }

        [PublicAPI]
        public void Add(IDisposable disposable)
        {
            LazyInitializeDisposableList();

            Assert.IsTrue(_disposables.Contains(disposable) == false);

            _disposables.Add(disposable);
        }

        [PublicAPI]
        public void Remove(IDisposable disposable)
        {
            Assert.IsNotNull(_disposables);

            _disposables.Remove(disposable);
        }

        private void StoreSpawnedObject<T>(T obj, IMemoryPool pool)
        {
            if (typeof(T).IsAssignableFrom(typeof(IDisposable)))
            {
                Add((IDisposable)obj);
            }
            else
            {
                // This allocation is ok because it's a struct
                var pair = new SpawnedObjectPoolPair
                {
                    Pool = pool,
                    Object = obj
                };

                if (_objectPoolPairs == null)
                {
                    _objectPoolPairs = ListPool<SpawnedObjectPoolPair>.Instance.Spawn();
                }
                _objectPoolPairs.Add(pair);
            }
        }

        [PublicAPI]
        public T Spawn<T>(IMemoryPool<T> pool)
        {
            var obj = pool.Spawn();
            StoreSpawnedObject(obj, pool);
            return obj;
        }

        [PublicAPI]
        public TValue Spawn<TValue, TParam1>(IMemoryPool<TParam1, TValue> pool, TParam1 p1)
        {
            var obj = pool.Spawn(p1);
            StoreSpawnedObject(obj, pool);
            return obj;
        }

        [PublicAPI]
        public TValue Spawn<TValue, TParam1, TParam2>(IMemoryPool<TParam1, TParam2, TValue> pool, TParam1 p1, TParam2 p2)
        {
            var obj = pool.Spawn(p1, p2);
            StoreSpawnedObject(obj, pool);
            return obj;
        }

        [PublicAPI]
        public TValue Spawn<TValue, TParam1, TParam2, TParam3>(IMemoryPool<TParam1, TParam2, TParam3, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3)
        {
            var obj = pool.Spawn(p1, p2, p3);
            StoreSpawnedObject(obj, pool);
            return obj;
        }

        [PublicAPI]
        public TValue Spawn<TValue, TParam1, TParam2, TParam3, TParam4>(IMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
            var obj = pool.Spawn(p1, p2, p3, p4);
            StoreSpawnedObject(obj, pool);
            return obj;
        }

        [PublicAPI]
        public TValue Spawn<TValue, TParam1, TParam2, TParam3, TParam4, TParam5>(IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
            var obj = pool.Spawn(p1, p2, p3, p4, p5);
            StoreSpawnedObject(obj, pool);
            return obj;
        }

        [PublicAPI]
        public TValue Spawn<TValue, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6>(IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6)
        {
            var obj = pool.Spawn(p1, p2, p3, p4, p5, p6);
            StoreSpawnedObject(obj, pool);
            return obj;
        }

        [PublicAPI]
        public TValue Spawn<TValue, TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7>(IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> pool, TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7)
        {
            var obj = pool.Spawn(p1, p2, p3, p4, p5, p6, p7);
            StoreSpawnedObject(obj, pool);
            return obj;
        }

        [PublicAPI]
        public List<T> SpawnList<T>(IEnumerable<T> elements)
        {
            var list = SpawnList<T>();
            list.AddRange(elements);
            return list;
        }

        [PublicAPI]
        public List<T> SpawnList<T>()
        {
            return Spawn(ListPool<T>.Instance);
        }

        [PublicAPI]
        public static DisposeBlock Spawn()
        {
            return _pool.Spawn();
        }

        public void Dispose()
        {
            _pool.Despawn(this);
        }

        private struct SpawnedObjectPoolPair
        {
            public IMemoryPool Pool;
            public object Object;
        }
    }
}
