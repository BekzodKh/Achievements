using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

using VContainer;
using VContainer.Unity;

using Core.Factories;
using Core.Pooling.Utils;
using Core.Utils;

namespace Core.Pooling
{
    public class PoolExceededFixedSizeException : Exception
    {
        public PoolExceededFixedSizeException(string errorMessage)
            : base(errorMessage)
        {
        }
    }

    public class MemoryPoolBase<TContract> : IMemoryPool, IDisposable, IInitializable where TContract : class
    {
        private Stack<TContract> _inactiveItems;

        private IFactory<TContract> _factory;
        private MemoryPoolSettings _settings;

        [Inject]
        public void Construct(IFactory<TContract> factory, MemoryPoolSettings settings)
        {
            _settings = settings;
            _factory = factory;
            _inactiveItems = new Stack<TContract>(_settings.InitialSize);
        }

        public void Initialize()
        {
            for (var i = 0; i < _settings.InitialSize; i++)
            {
                _inactiveItems.Push(AllocNew());
            }

#if UNITY_EDITOR
            StaticMemoryPoolRegistry.Add(this);
#endif
        }

        public IEnumerable<TContract> InactiveItems => _inactiveItems;

        public int NumTotal => NumInactive + NumActive;

        public int NumInactive => _inactiveItems.Count;

        public int NumActive { get; private set; }

        public Type ItemType => typeof(TContract);

        public void Dispose()
        {
#if UNITY_EDITOR
            StaticMemoryPoolRegistry.Remove(this);
#endif
        }

        void IMemoryPool.Despawn(object item)
        {
            Despawn((TContract)item);
        }

        public void Despawn(TContract item)
        {
            Assert.IsTrue(!_inactiveItems.Contains(item), $"Tried to return an item to pool {GetType()} twice");

            NumActive--;

            _inactiveItems.Push(item);

#if ZEN_INTERNAL_PROFILING
            using (ProfileTimers.CreateTimedBlock("User Code"))
#endif
#if UNITY_EDITOR
            using (ProfileBlock.Start("{0}.OnDespawned", GetType()))
#endif
            {
                OnDespawned(item);
            }

            if (_inactiveItems.Count > _settings.MaxSize)
            {
                Resize(_settings.MaxSize);
            }
        }

        private TContract AllocNew()
        {
            try
            {
                var item = _factory.Create();

                Assert.IsNotNull(item, $"Factory '{_factory.GetType()}' returned null value when creating via {GetType()}!");
                OnCreated(item);

                return item;
            }
            catch (Exception e)
            {
                throw new Exception($"Error during construction of type '{typeof(TContract)}' via {GetType()}.Create method!", e);
            }
        }

        public void Clear()
        {
            Resize(0);
        }

        public void ShrinkBy(int numToRemove)
        {
            Resize(_inactiveItems.Count - numToRemove);
        }

        public void ExpandBy(int numToAdd)
        {
            Resize(_inactiveItems.Count + numToAdd);
        }

        protected TContract GetInternal()
        {
            if (_inactiveItems.Count == 0)
            {
                ExpandPool();
                Assert.IsTrue(_inactiveItems.Count != 0);
            }

            var item = _inactiveItems.Pop();
            NumActive++;
            OnSpawned(item);
            return item;
        }

        public void Resize(int desiredPoolSize)
        {
            if (_inactiveItems.Count == desiredPoolSize)
            {
                return;
            }

            if (_settings.ExpandMethod == PoolExpandMethods.Disabled)
            {
                throw new PoolExceededFixedSizeException(
                    $"Pool factory '{GetType()}' attempted resize but pool set to fixed size of '{_inactiveItems.Count}'!");
            }

            Assert.IsTrue(desiredPoolSize >= 0, "Attempted to resize the pool to a negative amount");

            while (_inactiveItems.Count > desiredPoolSize)
            {
                OnDestroyed(_inactiveItems.Pop());
            }

            while (desiredPoolSize > _inactiveItems.Count)
            {
                _inactiveItems.Push(AllocNew());
            }

            Assert.AreEqual(_inactiveItems.Count, desiredPoolSize);
        }

        private void ExpandPool()
        {
            switch (_settings.ExpandMethod)
            {
                case PoolExpandMethods.Disabled:
                {
                    throw new PoolExceededFixedSizeException($"Pool factory '{GetType()}' exceeded its fixed size of '{_inactiveItems.Count}'!");
                }
                case PoolExpandMethods.OneAtATime:
                {
                    ExpandBy(1);
                    break;
                }
                case PoolExpandMethods.Double:
                {
                    ExpandBy(NumTotal == 0 ? 1 : NumTotal);
                    break;
                }
                default:
                {
                    throw new ArgumentException(nameof(PoolExpandMethods));
                }
            }
        }

        protected virtual void OnDespawned(TContract item)
        {
            // Optional
        }

        protected virtual void OnSpawned(TContract item)
        {
            // Optional
        }

        protected virtual void OnCreated(TContract item)
        {
            // Optional
        }

        protected virtual void OnDestroyed(TContract item)
        {
            // Optional
        }
    }
}
