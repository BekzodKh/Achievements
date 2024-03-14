using System.Collections.Generic;

using Newtonsoft.Json;

namespace Achievements.Models
{
    public class AchieveRewardModel
    {
        [JsonProperty] public List<string> NotSeenNewAchieveIds { get; } = new List<string>();
        [JsonIgnore] public List<string> NotClaimedAchieveIds { get; } = new List<string>();
        [JsonIgnore] public List<string> RewardedAchieveIds { get; } = new List<string>();
    }
}