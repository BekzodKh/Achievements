using System.Threading;

using Cysharp.Threading.Tasks;

using Core.Runtime.CommonViews;

namespace Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static IAsyncClickEventHandler GetAsyncClickEventHandler(this GameObjectButtonView button)
        {
            return new AsyncUnityEventHandler(button.onClick, button.GetCancellationTokenOnDestroy(), false);
        }

        public static IAsyncClickEventHandler GetAsyncClickEventHandler(this GameObjectButtonView button, CancellationToken cancellationToken)
        {
            return new AsyncUnityEventHandler(button.onClick, cancellationToken, false);
        }

        public static UniTask OnClickAsync(this GameObjectButtonView button)
        {
            return new AsyncUnityEventHandler(button.onClick, button.GetCancellationTokenOnDestroy(), true).OnInvokeAsync();
        }

        public static UniTask OnClickAsync(this GameObjectButtonView button, CancellationToken cancellationToken)
        {
            return new AsyncUnityEventHandler(button.onClick, cancellationToken, true).OnInvokeAsync();
        }

        public static IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable(this GameObjectButtonView button)
        {
            return new UnityEventHandlerAsyncEnumerable(button.onClick, button.GetCancellationTokenOnDestroy());
        }

        public static IUniTaskAsyncEnumerable<AsyncUnit> OnClickAsAsyncEnumerable(this GameObjectButtonView button, CancellationToken cancellationToken)
        {
            return new UnityEventHandlerAsyncEnumerable(button.onClick, cancellationToken);
        }
    }
}