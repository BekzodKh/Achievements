using System;

namespace Achievements.Data
{
    [Serializable]
    public class ProfileAchievements : IModel
    {
        public long ID { get; set; }
        public long ProfileId { get; set; }
        public string AchieveName { get; set; }
        public int Count { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}