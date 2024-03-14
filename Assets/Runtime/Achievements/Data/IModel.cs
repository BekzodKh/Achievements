using System;

namespace Achievements.Data
{
    public interface IModel
    {
        long ID { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}