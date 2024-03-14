using Newtonsoft.Json;

namespace Achievements.Models
{
    public struct AchieveNameAndGradeSnapshotContainer
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public int Count { get; set; }
    }
}