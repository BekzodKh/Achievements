using System.Collections.Generic;

using Newtonsoft.Json;

namespace Achievements.Models
{
    public class AchievesSnapshotContainer
    {
        [JsonProperty] public List<AchieveNameAndGradeSnapshotContainer> Achieves { get; set; } =
            new List<AchieveNameAndGradeSnapshotContainer>(1);
    }
}