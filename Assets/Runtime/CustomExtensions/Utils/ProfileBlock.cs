using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEngine.Profiling;
using System.Threading;
#endif

namespace Core.Utils
{
    public class ProfileBlock : IDisposable
    {
#if UNITY_EDITOR
        private static int _blockCount;
        private static ProfileBlock _instance = new ProfileBlock();
        private static readonly Dictionary<int, string> _nameCache = new Dictionary<int, string>();

        private ProfileBlock()
        {
        }

        // Required for disabling domain reload in enter the play mode feature. See: https://docs.unity3d.com/Manual/DomainReloading.html
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticValues()
        {
            if (!UnityEditor.EditorSettings.enterPlayModeOptionsEnabled)
            {
                return;
            }

            _instance = new ProfileBlock();
            _nameCache.Clear();
            _blockCount = 0;
        }

        [PublicAPI]
        public static Thread UnityMainThread { get; set; }

        [PublicAPI]
        public static Regex ProfilePattern { get; set; }

        private static int GetHashCode(object p1, object p2)
        {
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                hash = hash * 29 + p1.GetHashCode();
                hash = hash * 29 + p2.GetHashCode();
                return hash;
            }
        }

        private static int GetHashCode(object p1, object p2, object p3)
        {
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                hash = hash * 29 + p1.GetHashCode();
                hash = hash * 29 + p2.GetHashCode();
                hash = hash * 29 + p3.GetHashCode();
                return hash;
            }
        }

        [PublicAPI]
        public static ProfileBlock Start(string sampleNameFormat, object obj1, object obj2)
        {
#if ZEN_TESTS_OUTSIDE_UNITY
            return null;
#else
            if (UnityMainThread == null || !UnityMainThread.Equals(Thread.CurrentThread))
            {
                return null;
            }

            if (!Profiler.enabled)
            {
                return null;
            }

            // We need to ensure that we do not have per-frame allocations in ProfileBlock
            // to avoid infecting the test too much, so use a cache of formatted strings given
            // the input values
            // This only works if the input values do not change per frame
            var hash = GetHashCode(sampleNameFormat, obj1, obj2);

            if (!_nameCache.TryGetValue(hash, out var formatString))
            {
                formatString = string.Format(sampleNameFormat, obj1, obj2);
                _nameCache.Add(hash, formatString);
            }

            return StartInternal(formatString);
#endif
        }

        [PublicAPI]
        public static ProfileBlock Start(string sampleNameFormat, object obj)
        {
#if ZEN_TESTS_OUTSIDE_UNITY
            return null;
#else
            if (UnityMainThread == null || UnityMainThread.Equals(Thread.CurrentThread) == false)
            {
                return null;
            }

            if (Profiler.enabled == false)
            {
                return null;
            }

            // We need to ensure that we do not have per-frame allocations in ProfileBlock
            // to avoid infecting the test too much, so use a cache of formatted strings given
            // the input values
            // This only works if the input values do not change per frame
            var hash = GetHashCode(sampleNameFormat, obj);

            if (_nameCache.TryGetValue(hash, out var formatString))
                return StartInternal(formatString);

            formatString = string.Format(sampleNameFormat, obj);
            _nameCache.Add(hash, formatString);

            return StartInternal(formatString);
#endif
        }

        [PublicAPI]
        public static ProfileBlock Start(string sampleName)
        {
#if ZEN_TESTS_OUTSIDE_UNITY
            return null;
#else
            if (UnityMainThread == null || !UnityMainThread.Equals(Thread.CurrentThread))
            {
                return null;
            }

            return Profiler.enabled == false ? null : StartInternal(sampleName);
#endif
        }

        private static ProfileBlock StartInternal(string sampleName)
        {
            Assert.IsTrue(Profiler.enabled);

            if (ProfilePattern != null && ProfilePattern.Match(sampleName).Success == false)
                return null;

            Profiler.BeginSample(sampleName);
            _blockCount++;

            return _instance;
        }

        public void Dispose()
        {
            _blockCount--;

            Assert.IsTrue(_blockCount >= 0);

            Profiler.EndSample();
        }

#else
        ProfileBlock(string sampleName, bool rootBlock)
        {
        }

        ProfileBlock(string sampleName)
            : this(sampleName, false)
        {
        }

        public static Regex ProfilePattern
        {
            get;
            set;
        }

        public static ProfileBlock Start()
        {
            return null;
        }

        public static ProfileBlock Start(string sampleNameFormat, object obj1, object obj2)
        {
            return null;
        }

        // Remove the call completely for builds
        public static ProfileBlock Start(string sampleNameFormat, object obj)
        {
            return null;
        }

        // Remove the call completely for builds
        public static ProfileBlock Start(string sampleName)
        {
            return null;
        }

        public void Dispose()
        {
        }
#endif
    }
}