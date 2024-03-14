using System.Collections.Generic;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using Cysharp.Threading.Tasks;

namespace Core.Utils.AssetLoader
{
    public class AssetCacheLoaderService
    {
        private readonly Dictionary<object, AsyncOperationHandle<object>> _operationsDictionary = new Dictionary<object, AsyncOperationHandle<object>>();
        
        public async UniTask LoadContent(AssetReference reference)
        {
            var a = Addressables.LoadAssetAsync<object>(reference);
            await a;
            _operationsDictionary.Add(reference.RuntimeKey, a);
        }

        public void DisposeHandles()
        {
            foreach (var asyncOperationHandle in _operationsDictionary)
            {
                Addressables.Release(asyncOperationHandle.Value);
            }
            
            _operationsDictionary.Clear();
        }

        public T GetAsset<T>(object key)
        {
            return (T) _operationsDictionary[key].Result;
        }
    }
}