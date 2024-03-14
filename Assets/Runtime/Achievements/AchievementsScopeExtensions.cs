using VContainer;

using Achievements.Configs;
using Achievements.Models;
using Achievements.Service;
using Achievements.Views;

namespace Achievements.Controllers
{
    public static class AchievementsScopeExtensions
    {
        public static void UseAchievements(this IContainerBuilder builder,
            AchievesContainerConfig achievesContainerConfig, AchieveConfigsHolder achieveConfigsHolder)
        {
            builder.RegisterBuildCallback(achieveConfigsHolder.OnScopeBuild);
            
            builder.RegisterInstance(achievesContainerConfig).AsSelf();
            builder.RegisterInstance(achieveConfigsHolder).AsSelf();

            builder.Register<AchievesContextModel>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<AchievesModel>(Lifetime.Singleton);

            builder.Register<AchievementsService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}