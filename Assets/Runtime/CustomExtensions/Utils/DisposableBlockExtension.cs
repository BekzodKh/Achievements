using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Core.Utils
{
    public static class DisposableBlockExtension
    {
        [PublicAPI]
        public static void AddTo<T>(this IEnumerable<T> disposables, DisposeBlock disposeBlock) where T : IDisposable
        {
            disposeBlock.AddRange(disposables);
        }

        [PublicAPI]
        public static void AddTo(this IDisposable disposable, DisposeBlock disposeBlock)
        {
            disposeBlock.Add(disposable);
        }

        [PublicAPI]
        public static void RemoveFrom(this IDisposable disposable, DisposeBlock disposeBlock)
        {
            disposeBlock.Remove(disposable);
        }
    }
}