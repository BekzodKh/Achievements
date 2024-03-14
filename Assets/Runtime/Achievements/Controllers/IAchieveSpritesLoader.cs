using System.Collections.Generic;

using UnityEngine;

using Cysharp.Threading.Tasks;

using Achievements.Configs;
using Achievements.Models;

namespace Achievements.Controllers
{
    public interface IAchieveSpritesLoader
    {
        UniTask InitializeAsync(List<AchieveItem> allItems);
        IUniTaskAsyncEnumerable<Sprite> LoadSpritesAsync(AchieveCategory category);
        UniTask DisposingTabsAsync();
    }
}