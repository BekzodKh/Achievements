using System;

using Cysharp.Threading.Tasks;

namespace Core.Initialization
{
    public interface IInitializationQueue
    {
        void EnqueueFiredTask(Func<UniTaskVoid> task);

        void Enqueue(Func<UniTask> task);

        void Enqueue(Action action);
    }
}