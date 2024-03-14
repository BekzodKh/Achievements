using System.Collections.Generic;

using UnityEngine.Assertions;

using JetBrains.Annotations;

namespace Core.Pooling.Utils
{
    public class HashSetPool<T> : StaticMemoryPool<HashSet<T>>
    {
        private HashSetPool()
        {
            OnSpawnMethod = OnSpawned;
            OnDespawnedMethod = OnDespawned;
        }

        [PublicAPI]
        public static HashSetPool<T> Instance { get; } = new HashSetPool<T>();

#if UNITY_EDITOR
        // Required for disabling domain reload in enter the play mode feature. See: https://docs.unity3d.com/Manual/DomainReloading.html
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticValues()
        {
            if (!UnityEditor.EditorSettings.enterPlayModeOptionsEnabled)
            {
                return;
            }

            Instance.Clear();
        }
#endif

        private static void OnSpawned(HashSet<T> items)
        {
            Assert.IsTrue(items.Count == 0);
        }

        private static void OnDespawned(HashSet<T> items)
        {
            items.Clear();
        }
    }
}
