using System.Collections.Generic;

using UnityEngine.Assertions;

using JetBrains.Annotations;

namespace Core.Pooling.Utils
{
    [PublicAPI]
    public class DictionaryPool<TKey, TValue> : StaticMemoryPool<Dictionary<TKey, TValue>>
    {
        private DictionaryPool()
        {
            OnSpawnMethod = OnSpawned;
            OnDespawnedMethod = OnDespawned;
        }

        [PublicAPI]
        public static DictionaryPool<TKey, TValue> Instance { get; } = new DictionaryPool<TKey, TValue>();

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

        private static void OnSpawned(Dictionary<TKey, TValue> items)
        {
            Assert.IsTrue(items.Count == 0);
        }

        private static void OnDespawned(Dictionary<TKey, TValue> items)
        {
            items.Clear();
        }
    }
}

