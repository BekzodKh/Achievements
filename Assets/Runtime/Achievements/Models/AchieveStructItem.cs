using System;

using UnityEngine;
using UnityEngine.AddressableAssets;

using Sirenix.OdinInspector;

using Achievements.Configs;
using Achievements.Data;

namespace Achievements.Models
{
    [Serializable]
    public struct AchieveStructItem
    {
        public string Id => _id;
        public AchieveCategory Category => _category;
        public TargetRegion[] VisibleRegions => _visibleRegions;
        public AssetReferenceSprite AchieveIconReference => _achieveIconReference;
        public AssetReferenceSprite AchieveShadowIconReference => _achieveShadowIconReference;
        public int MaxCountAchieves => _maxCountAchieves;

        [Title("$GetTitle", TitleAlignment = TitleAlignments.Centered)]
        [SerializeField] private string _id;
        [SerializeField, FoldoutGroup("Data")] private AchieveCategory _category;
        [SerializeField, FoldoutGroup("Data")] private TargetRegion[] _visibleRegions;

        [SerializeField, FoldoutGroup("Data")]
        private AssetReferenceSprite _achieveIconReference;

        [SerializeField, FoldoutGroup("Data")]
        private AssetReferenceSprite _achieveShadowIconReference;
        [SerializeField, FoldoutGroup("Data")] private int _maxCountAchieves;

#if UNITY_EDITOR
        public AchieveStructItem(string achieveId, AchieveCategory category)
        {
            _id = achieveId;
            _category = category;
            _achieveIconReference = default;
            _achieveShadowIconReference = default;
            _maxCountAchieves = 1;
            _visibleRegions = Array.Empty<TargetRegion>();
        }
        
        private string GetTitle()
        {
            return "Test Title";
        }
#endif
    }
}