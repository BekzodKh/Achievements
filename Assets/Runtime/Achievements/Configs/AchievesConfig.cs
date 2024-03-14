using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Achievements.Conditions;

namespace Achievements.Configs
{
    [CreateAssetMenu(fileName = nameof(AchievesConfig), menuName = "Achieves/" + nameof(AchievesConfig))]
    public class AchievesConfig : ScriptableObject
    {
        public Dictionary<AchieveTypes, List<IAchieveCondition>> AchieveConditionsMap =>
            _achieveDatas.ToDictionary(k => k.AchieveType, v => v.AchieveConditions);

        [SerializeField] private List<AchieveData> _achieveDatas;

#if UNITY_EDITOR
        public void AddAchieveData(AchieveData data)
        {
            _achieveDatas.Add(data);
        }

        public void SetConditionToAchieve(string achieveId, IAchieveCondition condition)
        {
            var achieveData = _achieveDatas.Find(x => x.AchieveType.AchieveId == achieveId);
            achieveData.AchieveConditions.Clear();
            achieveData.AchieveConditions.Add(condition);
        }

        public void AddConditionToAchieve(string achieveId, IAchieveCondition condition)
        {
            var achieveData = _achieveDatas.Find(x => x.AchieveType.AchieveId == achieveId);
            achieveData.AchieveConditions.Add(condition);
        }

        public void ReplaceConditionToAchieve(string achieveId, IAchieveCondition prevCondition, IAchieveCondition newCondition)
        {
            var achieveData = _achieveDatas.Find(x => x.AchieveType.AchieveId == achieveId);
            achieveData.AchieveConditions.Remove(prevCondition);
            achieveData.AchieveConditions.Add(newCondition);
        }
#endif
    }
}