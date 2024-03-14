using System.Collections.Generic;

using Newtonsoft.Json;

namespace Achievements.Models
{
    public class AchieveCountReachModel
    {
        [JsonProperty] public Dictionary<string, int> CountByAchieveId = new Dictionary<string, int>();
    }
}