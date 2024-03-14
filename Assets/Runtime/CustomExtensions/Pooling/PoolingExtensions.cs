using System;

using UnityEngine;

using VContainer;
using VContainer.Unity;

using Core.Factories;

namespace Core.Pooling
{
    public static class PoolingExtensions
    {
        public static RegistrationBuilder UseMonoPoolWithFactory<TPrefab, TPool>(this IContainerBuilder builder, TPrefab prefab, Action<MonoMemoryPoolSettings> settingsBuilder = null)
            where TPrefab : Component
            where TPool : IMemoryPool<TPrefab>
        {
            var settings = new MonoMemoryPoolSettings();
            settingsBuilder?.Invoke(settings);

            builder.Register<StaticPrefabFactory<TPrefab>>(Lifetime.Singleton)
                .As<IFactory<TPrefab>>()
                .WithParameter(prefab)
                .WithParameter(settings.Parent);

            return builder.Register<TPool>(Lifetime.Singleton)
                .WithParameter((MemoryPoolSettings)settings)
                .As<TPool>()
                .As<IMemoryPool<TPrefab>>()
                .As<IInitializable>()
                .As<IDisposable>();
        }

        public static RegistrationBuilder UseMonoPoolWithRandomFactory<TPrefab, TPool>(this IContainerBuilder builder, TPrefab[] prefabs, Action<RandomMemoryPoolSettings> settingsBuilder = null)
            where TPrefab : Component
            where TPool : IMemoryPool<TPrefab>
        {
            var settings = new RandomMemoryPoolSettings();
            settingsBuilder?.Invoke(settings);

            builder.Register<RandomStaticPrefabFactory<TPrefab>>(Lifetime.Singleton)
                .As<IFactory<TPrefab>>()
                .WithParameter(prefabs)
                .WithParameter(settings.Parent)
                .WithParameter(settings.RandomIndexGetter);

            return builder.Register<TPool>(Lifetime.Singleton)
                .WithParameter((MemoryPoolSettings)settings)
                .As<TPool>()
                .As<IMemoryPool<TPrefab>>()
                .As<IInitializable>()
                .As<IDisposable>();
        }

        public static RegistrationBuilder UseMonoPoolFactory<TPrefab>(this IContainerBuilder builder, Action<MemoryPoolSettings> settingsBuilder = null)
            where TPrefab : Component
        {
            var settings = new MemoryPoolSettings();
            settingsBuilder?.Invoke(settings);

            return builder.Register<MonoMemoryPoolFactory<TPrefab>>(Lifetime.Singleton)
                .As<IFactory<TPrefab, IMemoryPool<TPrefab>>>()
                .As<IFactory<TPrefab, MonoMemoryPool<TPrefab>>>()
                .WithParameter(settings);
        }

        public static RegistrationBuilder UseMonoPool<TPrefab, TPool>(this IContainerBuilder builder, Action<MemoryPoolSettings> settingsBuilder = null)
            where TPrefab : Component
            where TPool : IMemoryPool<TPrefab>
        {
            var settings = new MemoryPoolSettings();
            settingsBuilder?.Invoke(settings);

            return builder.Register<TPool>(Lifetime.Singleton)
                .WithParameter(settings)
                .As<TPool>()
                .As<IMemoryPool<TPrefab>>()
                .As<IInitializable>()
                .As<IDisposable>();
        }

        public static RegistrationBuilder UsePoolWithFactory<T, TPool>(this IContainerBuilder builder, Action<MemoryPoolSettings> settingsBuilder = null)
            where TPool : IMemoryPool<T>
            where T : class, new()
        {
            var settings = new MemoryPoolSettings();
            settingsBuilder?.Invoke(settings);

            builder.Register<ObjectFactory<T>>(Lifetime.Singleton)
                .As<IFactory<T>>();

            return builder.Register<TPool>(Lifetime.Singleton)
                .WithParameter(settings)
                .As<TPool>()
                .As<IMemoryPool<T>>()
                .As<IInitializable>()
                .As<IDisposable>();
        }

        public static RegistrationBuilder UsePool<T, TPool>(this IContainerBuilder builder, Action<MemoryPoolSettings> settingsBuilder = null)
            where TPool : IMemoryPool<T>
            where T : class, new()
        {
            var settings = new MemoryPoolSettings();
            settingsBuilder?.Invoke(settings);

            return builder.Register<TPool>(Lifetime.Singleton)
                .WithParameter(settings)
                .As<TPool>()
                .As<IMemoryPool<T>>()
                .As<IInitializable>()
                .As<IDisposable>();
        }
    }
}