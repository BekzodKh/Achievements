using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using Achievements.Configs;
using Achievements.Models;

namespace Achievements.Controllers
{
    public sealed class FullAchieveLoader : IAchieveSpritesLoader
    {
        private Dictionary<AchieveCategory, List<AsyncOperationHandle<Sprite>>> _iconsOperations;

        public async UniTask InitializeAsync(List<AchieveItem> allItems)
        {
            _iconsOperations = new Dictionary<AchieveCategory, List<AsyncOperationHandle<Sprite>>>
            {
                [AchieveCategory.MiniGames] = new List<AsyncOperationHandle<Sprite>>(),
                [AchieveCategory.LearningProgress] = new List<AsyncOperationHandle<Sprite>>(),
                [AchieveCategory.Collections] = new List<AsyncOperationHandle<Sprite>>(),
            };

            foreach (var allItem in allItems)
            {
                _iconsOperations[allItem.Category]
                    .Add(Addressables.LoadAssetAsync<Sprite>(allItem.AssetToShow.RuntimeKey));
            }

            await UniTask.WhenAll(_iconsOperations.Values.SelectMany(x => x).Select(x => x.ToUniTask()));
        }

        public IUniTaskAsyncEnumerable<Sprite> LoadSpritesAsync(AchieveCategory category)
        {
            var miniGameIcons = _iconsOperations[category];

            return UniTaskAsyncEnumerable.Create<Sprite>(async (writer, y) =>
            {
                foreach (var operation in miniGameIcons)
                {
                    await writer.YieldAsync(await operation);
                }
            });
        }

        public async UniTask DisposingTabsAsync()
        {
            foreach (var operation in _iconsOperations.Values.SelectMany(operations => operations))
            {
                Addressables.Release(operation);
            }

            await Resources.UnloadUnusedAssets();
        }
    }
}