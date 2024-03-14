using System.Collections.Generic;
using System.Linq;

namespace Achievements.Models
{
    public class AchieveCollection
    {
        private readonly List<AchieveItem> _achieveItems = new List<AchieveItem>(5);

        public bool AddItemIncrement(AchieveItem item)
        {
            if (_achieveItems.Any(achieveItem => achieveItem.AchieveID.Equals(item.AchieveID)))
            {
                if(item.CountOfAchieves >= item.MaxCountAchieves)
                {
                    return false;
                }
                else
                {
                    _achieveItems.Remove(item);
                }
            }
            
            item.CountOfAchieves++;
            
            _achieveItems.Add(item);

            return true;
        }
        
        public bool AddItem(AchieveItem item)
        {
            var achieveItem = _achieveItems.Find(achieveItem => achieveItem.AchieveID.Equals(item.AchieveID));

            if (achieveItem != null)
            {
                if (achieveItem.CountOfAchieves >= item.CountOfAchieves)
                {
                    return false;
                }
                else
                {
                    _achieveItems.Remove(achieveItem);
                }
            }
            
            _achieveItems.Add(item);

            return true;
        }

        public List<AchieveItem> Get()
        {
            return _achieveItems;
        }
    }
}