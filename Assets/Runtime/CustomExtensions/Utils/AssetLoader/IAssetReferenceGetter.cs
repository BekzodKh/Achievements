using UnityEngine.AddressableAssets;

namespace Core.Utils.AssetLoader
{
    public interface IAssetReferenceGetter
    {
        AssetReference[] GetAssets();
    }
}