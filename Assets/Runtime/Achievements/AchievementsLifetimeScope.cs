using UnityEngine;

using VContainer;
using VContainer.Unity;

using Achievements.Configs;
using Achievements.Controllers;
using Achievements.Views;

namespace Achievements.LifetimeScopes
{
    public class AchievementsLifetimeScope : LifetimeScope
    {
        [SerializeField] private AchieveConfigsHolder _achieveConfigsHolder;
        [SerializeField] private AchievesContainerConfig _achievesContainerConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.UseAchievements(_achievesContainerConfig, _achieveConfigsHolder);
            
            builder.Register<FullAchieveLoader>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}