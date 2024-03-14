using Cysharp.Threading.Tasks;

namespace Core.Initialization
{
    public interface IInitializationQueueExecutor
    {
        UniTask Execute();
    }
}