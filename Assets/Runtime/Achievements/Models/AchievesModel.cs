using System;

namespace Achievements.Models
{
    public class AchievesModel
    {
        public event Action AchievesTracked = delegate { };
        public event Action ColorByCodeBackToMenu = delegate { };
        
        public bool IsLpTracking { get; set; }

        public void InvokeAchievesTracked()
        {
            AchievesTracked.Invoke();
        }

        public void InvokeColorByCodeBackToMenu()
        {
            ColorByCodeBackToMenu.Invoke();
        }
    }
}