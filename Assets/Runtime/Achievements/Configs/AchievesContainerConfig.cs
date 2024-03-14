using System.Collections.Generic;

using UnityEngine;

using Achievements.Models;

namespace Achievements.Configs
{
    [CreateAssetMenu(fileName = "AchievesContainerConfig", menuName = "Achieves/AchievesContainerConfig", order = 0)]
    public class AchievesContainerConfig : ScriptableObject
    {
        [SerializeField] private List<AchieveStructItem> _achieveItems;

        public IReadOnlyList<AchieveStructItem> AchieveItems => _achieveItems;

#if UNITY_EDITOR
        public void AddNewAchieveItem(AchieveStructItem item)
        {
            _achieveItems.Add(item);
        }
        public void ReplaceAchieveItem(int prevItemIndex, AchieveStructItem item)
        {
            _achieveItems.RemoveAt(prevItemIndex);
            _achieveItems.Insert(prevItemIndex, item);
        }
#endif
        
    }
}