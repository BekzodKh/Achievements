using JetBrains.Annotations;

using UnityEngine;

namespace Core.Pooling
{
    [PublicAPI]
    public class MonoMemoryPoolSettings : MemoryPoolSettings
    {
        public Transform Parent { get; set; }
    }
}