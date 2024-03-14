using System.Collections.Generic;
using System.Linq;
using System.Threading;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

using Achievements.Configs;
using Achievements.Models;

namespace Achievements.Controllers
{
    public class SeparateAchieveLoader : IAchieveSpritesLoader
    {
        private Dictionary<AchieveCategory, TabState> _tabState;
        private Queue<AchieveCategory> _openingTabs;
        private List<AchieveItem> _icons;
        
        public async UniTask InitializeAsync(List<AchieveItem> allItems)
        {
            _openingTabs = new Queue<AchieveCategory>(3);
            _tabState = new Dictionary<AchieveCategory, TabState>(3)
            {
                [AchieveCategory.MiniGames] = new TabState(),
                [AchieveCategory.LearningProgress] = new TabState(),
                [AchieveCategory.Collections] = new TabState(),
            };

            _icons = allItems;
            await UniTask.CompletedTask;
        }

        public IUniTaskAsyncEnumerable<Sprite> LoadSpritesAsync(AchieveCategory category)
        {
            DisposingTabsAsync().Forget();
            _openingTabs.Enqueue(category);
            var miniGameIcons = _icons.Where(x => x.Category == category).ToList();

            var existed = GetExistedOperations(category, miniGameIcons);
            
            var newOperations = miniGameIcons.Skip(existed.Count)
                .Select(x => Addressables.LoadAssetAsync<Sprite>(x.AssetToShow.RuntimeKey));
            
            return UniTaskAsyncEnumerable.Create<Sprite>(async (writer, y) =>
            {
                if (_tabState.TryGetValue(category, out var value) && value.Processed)
                {
                    await UniTask.WaitWhile(() => _tabState[category].Processed);
                }

                CancelDisposing(category);

                _tabState[category].Processed = true;

                foreach (var operation in existed)
                {
                    var sprite = await operation;
                    if (IsDisposing(category))
                    {
                        return;
                    }

                    await writer.YieldAsync(sprite);
                }

                foreach (var operation in newOperations.ToList())
                {
                    _tabState[category].LoadingProcesses.Add(operation);
                    var sprite = await operation;
                    if (IsDisposing(category))
                    {
                        return;
                    }

                    await writer.YieldAsync(sprite);
                }

                _tabState[category].Processed = false;
            });
        }

        private bool IsDisposing(AchieveCategory category)
        {
            if (_tabState[category].DisposingCancellation == null)
            {
                return false;
            }

            DisposeOperations(category).Forget();
            _tabState[category].Processed = false;
            return true;

        }

        public async UniTask DisposingTabsAsync()
        {
            var disposedTasks = new List<UniTask>();

            while (_openingTabs.Count > 0)
            {
                var category = _openingTabs.Dequeue();
                _tabState[category].DisposingCancellation = new CancellationTokenSource();
                if (_tabState[category].LoadingProcesses.All(x => x.IsDone))
                {
                    disposedTasks.Add(DisposeOperations(category));
                }
            }

            await UniTask.WhenAll(disposedTasks);
        }


        private void CancelDisposing(AchieveCategory category)
        {
            _tabState[category].DisposingCancellation?.Cancel();
            _tabState[category].DisposingCancellation?.Dispose();
            _tabState[category].DisposingCancellation = null;
        }

        private List<AsyncOperationHandle<Sprite>> GetExistedOperations(AchieveCategory category,
            List<AchieveItem> miniGameIcons)
        {
            if (_tabState[category].LoadingProcesses == null || _tabState[category].LoadingProcesses.Count == 0)
            {
                return _tabState[category].LoadingProcesses =
                    new List<AsyncOperationHandle<Sprite>>(miniGameIcons.Count);
            }
            
            if (_tabState[category].LoadingProcesses.Any(x => !x.IsValid()))
            {
                _tabState[category].LoadingProcesses.Clear();
            }
            
            return _tabState[category].LoadingProcesses;
        }

        private async UniTask DisposeOperations(AchieveCategory category)
        {
            foreach (var operationHandle in _tabState[category].LoadingProcesses)
            {
                if (!operationHandle.IsValid())
                {
                    Debug.LogWarning("Cannot release a null or unloaded asset.");
                    continue;
                }

                Resources.UnloadAsset(operationHandle.Result);
                Addressables.Release(operationHandle);
            }
            _tabState[category].LoadingProcesses.Clear();
            await Resources.UnloadUnusedAssets();
        }

        private class TabState
        {
            public List<AsyncOperationHandle<Sprite>> LoadingProcesses { get; set; }
            public CancellationTokenSource DisposingCancellation { get; set; }
            public bool Processed { get; set; }
        }
    }
}