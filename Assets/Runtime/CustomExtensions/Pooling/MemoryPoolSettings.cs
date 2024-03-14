using JetBrains.Annotations;

namespace Core.Pooling
{
    [PublicAPI]
    public class MemoryPoolSettings
    {
        public int InitialSize { get; set; }
        public int MaxSize { get; set; }
        public PoolExpandMethods ExpandMethod { get; set; }

        public MemoryPoolSettings()
        {
            InitialSize = 0;
            MaxSize = int.MaxValue;
            ExpandMethod = PoolExpandMethods.OneAtATime;
        }

        public MemoryPoolSettings(int initialSize, int maxSize, PoolExpandMethods expandMethod)
        {
            InitialSize = initialSize;
            MaxSize = maxSize;
            ExpandMethod = expandMethod;
        }
    }
}