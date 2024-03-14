using System.Collections.Generic;

using JetBrains.Annotations;

namespace Core.Pooling.Utils
{
    public class ListPool<T> : StaticMemoryPool<List<T>>
    {
        private ListPool()
        {
            OnDespawnedMethod = OnDespawned;
        }

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

        [PublicAPI]
        public static ListPool<T> Instance { get; } = new ListPool<T>();

        private static void OnDespawned(List<T> list)
        {
            list.Clear();
        }
    }
}
