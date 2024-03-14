using UnityEngine;

using JetBrains.Annotations;

using Core.Utils.IndexGenerators;

namespace Core.Pooling
{
    [PublicAPI]
    public class RandomMemoryPoolSettings : MemoryPoolSettings
    {
        public Transform Parent { get; set; }
        public IIndexGenerator RandomIndexGetter { get; set; }
    }
}