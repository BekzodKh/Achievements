using UnityEngine;

using VContainer;

using Achievements.Configs;

namespace Achievements.Views
{
    public class AchieveConfigsHolder : MonoBehaviour
    {
        public AchievesConfig MiniGamesAchieveConfig => _miniGamesAchieveConfig;
        public AchievesConfig ColoringAchieveConfig => _coloringAchieveConfig;
        public AchievesConfig ProgressAchieveConfig => _progressAchieveConfig;
        public AchievesConfig CollectsAchieveConfig => _collectsAchieveConfig;
        
        [SerializeField] private AchievesConfig _miniGamesAchieveConfig;
        [SerializeField] private AchievesConfig _coloringAchieveConfig;
        [SerializeField] private AchievesConfig _progressAchieveConfig;
        [SerializeField] private AchievesConfig _collectsAchieveConfig;

        public void OnScopeBuild(IObjectResolver resolver)
        {
            SetResolverToConditions(_miniGamesAchieveConfig, resolver);
            SetResolverToConditions(_coloringAchieveConfig, resolver);
            SetResolverToConditions(_progressAchieveConfig, resolver);
            SetResolverToConditions(_collectsAchieveConfig, resolver);
        }

        private void SetResolverToConditions(AchievesConfig config, IObjectResolver resolver)
        {
            foreach (var conditions in config.AchieveConditionsMap.Values)
            {
                foreach (var condition in conditions)
                {
                    condition.OnScopeBuilt(resolver);
                }
            }
        }
    }
}