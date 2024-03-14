using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

using Core.Pooling.Utils;

namespace Core.Pooling
{
    public abstract class StaticMemoryPoolBaseBase<TValue> : IDespawnableMemoryPool<TValue>, IDisposable
        where TValue : class
    {
        // I also tried using ConcurrentBag instead of Stack + lock here but that performed much much worse
        private readonly Stack<TValue> _stack = new Stack<TValue>();

        private Action<TValue> _onDespawnedMethod;
        private int _activeCount;

#if ZEN_MULTITHREADING
        protected readonly object _locker = new object();
#endif

        protected StaticMemoryPoolBaseBase(Action<TValue> onDespawnedMethod)
        {
            _onDespawnedMethod = onDespawnedMethod;

#if UNITY_EDITOR
            StaticMemoryPoolRegistry.Add(this);
#endif
        }

        protected Action<TValue> OnDespawnedMethod
        {
            set => _onDespawnedMethod = value;
        }

        public int NumTotal => NumInactive + NumActive;

        public int NumActive
        {
            get
            {
#if ZEN_MULTITHREADING
                lock (_locker)
#endif
                {
                    return _activeCount;
                }
            }
        }

        public int NumInactive
        {
            get
            {
#if ZEN_MULTITHREADING
                lock (_locker)
#endif
                {
                    return _stack.Count;
                }
            }
        }

        public Type ItemType => typeof(TValue);

        public void Resize(int desiredPoolSize)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                ResizeInternal(desiredPoolSize);
            }
        }

        // We assume here that we're in a lock
        private void ResizeInternal(int desiredPoolSize)
        {
            Assert.IsTrue(desiredPoolSize >= 0, "Attempted to resize the pool to a negative amount");

            while (_stack.Count > desiredPoolSize)
            {
                _stack.Pop();
            }

            while (desiredPoolSize > _stack.Count)
            {
                _stack.Push(Alloc());
            }

            Assert.AreEqual(_stack.Count, desiredPoolSize);
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            StaticMemoryPoolRegistry.Remove(this);
#endif
        }

        public void ClearActiveCount()
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                _activeCount = 0;
            }
        }

        public void Clear()
        {
            Resize(0);
        }

        public void ShrinkBy(int numToRemove)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                ResizeInternal(_stack.Count - numToRemove);
            }
        }

        public void ExpandBy(int numToAdd)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                ResizeInternal(_stack.Count + numToAdd);
            }
        }

        // We assume here that we're in a lock
        protected TValue SpawnInternal()
        {
            var element = _stack.Count == 0 ? Alloc() : _stack.Pop();

            _activeCount++;
            return element;
        }

        void IMemoryPool.Despawn(object item)
        {
            Despawn((TValue)item);
        }

        public void Despawn(TValue element)
        {
            _onDespawnedMethod?.Invoke(element);

#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                Assert.IsTrue(!_stack.Contains(element), "Attempted to despawn element twice!");

                _activeCount--;
                _stack.Push(element);
            }
        }

        protected abstract TValue Alloc();
    }

    public abstract class StaticMemoryPoolBase<TValue> : StaticMemoryPoolBaseBase<TValue>
        where TValue : class, new()
    {
        protected StaticMemoryPoolBase(Action<TValue> onDespawnedMethod)
            : base(onDespawnedMethod)
        {
        }

        protected override TValue Alloc()
        {
            return new TValue();
        }
    }

    // Zero parameters
    public class StaticMemoryPool<TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TValue>
        where TValue : class, new()
    {
        private Action<TValue> _onSpawnMethod;

        public StaticMemoryPool(
            Action<TValue> onSpawnMethod = null, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            _onSpawnMethod = onSpawnMethod;
        }

        protected Action<TValue> OnSpawnMethod
        {
            set => _onSpawnMethod = value;
        }

        public TValue Spawn()
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                var item = SpawnInternal();

                _onSpawnMethod?.Invoke(item);

                return item;
            }
        }
    }

    // One parameter
    public class StaticMemoryPool<TParam1, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TValue>
        where TValue : class, new()
    {
        private Action<TParam1, TValue> _onSpawnMethod;

        protected StaticMemoryPool(
            Action<TParam1, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TValue> OnSpawnMethod
        {
            set => _onSpawnMethod = value;
        }

        public TValue Spawn(TParam1 param)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                var item = SpawnInternal();

                _onSpawnMethod?.Invoke(param, item);

                return item;
            }
        }
    }

    // Two parameter
    public class StaticMemoryPool<TParam1, TParam2, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TValue>
        where TValue : class, new()
    {
        private Action<TParam1, TParam2, TValue> _onSpawnMethod;

        protected StaticMemoryPool(
            Action<TParam1, TParam2, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TParam2, TValue> OnSpawnMethod
        {
            set => _onSpawnMethod = value;
        }

        public TValue Spawn(TParam1 p1, TParam2 p2)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                var item = SpawnInternal();

                _onSpawnMethod?.Invoke(p1, p2, item);

                return item;
            }
        }
    }

    // Three parameters
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TValue>
        where TValue : class, new()
    {
        private Action<TParam1, TParam2, TParam3, TValue> _onSpawnMethod;

        protected StaticMemoryPool(
            Action<TParam1, TParam2, TParam3, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TParam2, TParam3, TValue> OnSpawnMethod
        {
            set => _onSpawnMethod = value;
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                var item = SpawnInternal();

                _onSpawnMethod?.Invoke(p1, p2, p3, item);

                return item;
            }
        }
    }

    // Four parameters
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TValue>
        where TValue : class, new()
    {
        private Action<TParam1, TParam2, TParam3, TParam4, TValue> _onSpawnMethod;

        protected StaticMemoryPool(
            Action<TParam1, TParam2, TParam3, TParam4, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TParam2, TParam3, TParam4, TValue> OnSpawnMethod
        {
            set => _onSpawnMethod = value;
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                var item = SpawnInternal();

                _onSpawnMethod?.Invoke(p1, p2, p3, p4, item);

                return item;
            }
        }
    }

    // Five parameters
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TValue>
        where TValue : class, new()
    {
        private Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> _onSpawnMethod;

        protected StaticMemoryPool(
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TParam2, TParam3, TParam4, TParam5, TValue> OnSpawnMethod
        {
            set => _onSpawnMethod = value;
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                var item = SpawnInternal();

                _onSpawnMethod?.Invoke(p1, p2, p3, p4, p5, item);

                return item;
            }
        }
    }

    // Six parameters
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue>
        where TValue : class, new()
    {
        private Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> _onSpawnMethod;

        protected StaticMemoryPool(
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TValue> OnSpawnMethod
        {
            set => _onSpawnMethod = value;
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                var item = SpawnInternal();

                _onSpawnMethod?.Invoke(p1, p2, p3, p4, p5, p6, item);

                return item;
            }
        }
    }

    // Seven parameters
    public class StaticMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> : StaticMemoryPoolBase<TValue>, IMemoryPool<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue>
        where TValue : class, new()
    {
        private Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> _onSpawnMethod;

        protected StaticMemoryPool(
            Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> onSpawnMethod, Action<TValue> onDespawnedMethod = null)
            : base(onDespawnedMethod)
        {
            // What's the point of having a param otherwise?
            Assert.IsNotNull(onSpawnMethod);
            _onSpawnMethod = onSpawnMethod;
        }

        public Action<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TValue> OnSpawnMethod
        {
            set => _onSpawnMethod = value;
        }

        public TValue Spawn(TParam1 p1, TParam2 p2, TParam3 p3, TParam4 p4, TParam5 p5, TParam6 p6, TParam7 p7)
        {
#if ZEN_MULTITHREADING
            lock (_locker)
#endif
            {
                var item = SpawnInternal();

                _onSpawnMethod?.Invoke(p1, p2, p3, p4, p5, p6, p7, item);

                return item;
            }
        }
    }
}
