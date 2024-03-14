using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;

using JetBrains.Annotations;

namespace Core.Initialization
{
    [UsedImplicitly]
    public class InitializationQueue : IInitializationQueue, IInitializationQueueExecutor
    {
        private readonly Queue<Func<UniTaskVoid>> _firedTaskQueue;
        private readonly Queue<Func<UniTask>> _taskQueue;
        private readonly Queue<Action> _actionQueue;

        public InitializationQueue()
        {
            _firedTaskQueue = new Queue<Func<UniTaskVoid>>();
            _taskQueue = new Queue<Func<UniTask>>();
            _actionQueue = new Queue<Action>();
        }

        async UniTask IInitializationQueueExecutor.Execute()
        {
            for (int i = 0, length = _taskQueue.Count; i < length; i++)
            {
                await _taskQueue.Dequeue().Invoke();
            }

            for (int i = 0, length = _firedTaskQueue.Count; i < length; i++)
            {
                _firedTaskQueue.Dequeue().Invoke().Forget();
            }

            for (int i = 0, length = _actionQueue.Count; i < length; i++)
            {
                _actionQueue.Dequeue().Invoke();
                await UniTask.DelayFrame(2);
            }
        }

        void IInitializationQueue.Enqueue(Func<UniTask> task)
        {
            _taskQueue.Enqueue(task);
        }

        void IInitializationQueue.EnqueueFiredTask(Func<UniTaskVoid> task)
        {
            _firedTaskQueue.Enqueue(task);
        }

        void IInitializationQueue.Enqueue(Action action)
        {
            _actionQueue.Enqueue(action);
        }
    }
}