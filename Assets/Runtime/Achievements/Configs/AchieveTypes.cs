using System;

using UnityEngine;

using Sirenix.OdinInspector;

namespace Achievements.Configs
{
    [Serializable] public struct AchieveTypes
    {
        public string AchieveId => _achieveId;

        [SerializeField] private AchievesContainerConfig _achievesContainerConfig;
#if UNITY_EDITOR
        [ValueDropdown("GetTypes")]
#endif
        [SerializeField]
        private string _achieveId;

        public AchieveTypes(AchievesContainerConfig config, string id)
        {
            _achievesContainerConfig = config;
            _achieveId = id;
        }

#if UNITY_EDITOR
        private string[] GetTypes()
        {
            string[] ids = new string[_achievesContainerConfig.AchieveItems.Count];

            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = _achievesContainerConfig.AchieveItems[i].Id;
            }
            
            return ids;
        }
#endif
    }
}