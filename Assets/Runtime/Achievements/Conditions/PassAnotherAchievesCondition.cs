using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using VContainer;

using Achievements.Configs;
using Achievements.Service;

namespace Achievements.Conditions
{
    public class PassAnotherAchievesCondition : IAchieveCondition
    {
        [SerializeField] private List<AchieveTypes> _achievesToPass = new List<AchieveTypes>();

        private AchievementsService _achievementsService;

        public bool CheckIsConditionMet()
        {
            return _achievesToPass.Select(achieveType => _achievementsService.GetAchieveById(achieveType.AchieveId))
                .All(achieve => achieve is { Passed: true });
        }

        public void OnScopeBuilt(IObjectResolver resolver)
        {
            _achievementsService = resolver.Resolve<AchievementsService>();
        }

#if UNITY_EDITOR
        public void AddAchieveToPass(AchieveTypes achieve)
        {
            _achievesToPass.Add(achieve);
        }
#endif
    }
}