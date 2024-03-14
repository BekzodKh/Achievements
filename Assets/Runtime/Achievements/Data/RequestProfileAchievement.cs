using Newtonsoft.Json;

namespace Achievements.Data
{
    public class RequestProfileAchievement
    {
        [JsonProperty("profileId")] public long ProfileId { get; set; }
        [JsonProperty("achieveName")] public string AchieveName { get; set; }
        [JsonProperty("count")] public int Count { get; set; }
    }
}