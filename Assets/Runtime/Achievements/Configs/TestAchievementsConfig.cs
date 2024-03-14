using UnityEngine;

namespace Achievements.Configs
{
    [CreateAssetMenu(fileName = "TestAchievementsConfig", menuName = "Achieves/TestAchievementsConfig", order = 0)]
    public class TestAchievementsConfig : ScriptableObject
    {
        [SerializeField] private AchieveTypes _achieveTypes;
    }
}