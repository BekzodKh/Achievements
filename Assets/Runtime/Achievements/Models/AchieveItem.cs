using UnityEngine.AddressableAssets;

using Achievements.Configs;
using Achievements.Data;

namespace Achievements.Models
{
    public class AchieveItem
    {
        public string AchieveID { get; }
        public string Name => GetName();
        public AssetReferenceSprite AchieveIconReference { get; }
        public AssetReferenceSprite AchieveShadowIconReference { get; }
        public int MaxCountAchieves { get; }
        public int CountOfAchieves { get; set; } = 0;

        public AchieveCategory Category { get; }
        public TargetRegion[] VisibleRegions { get; }

        public bool Passed => CountOfAchieves == MaxCountAchieves;

        public AssetReferenceSprite AssetToShow => Passed ? AchieveIconReference : AchieveShadowIconReference;

        public AchieveItem(string id, AssetReferenceSprite iconReference,
            AssetReferenceSprite achieveShadowIconReference, int achievesCount, int maxCountAchieves, 
            AchieveCategory category, TargetRegion[] visibleRegions)
        {
            AchieveID = id;
            AchieveIconReference = iconReference;
            AchieveShadowIconReference = achieveShadowIconReference;
            MaxCountAchieves = maxCountAchieves;
            CountOfAchieves = achievesCount;
            Category = category;
            VisibleRegions = visibleRegions;
        }

        private string GetName()
        {
            return "test";
        }
    }
}