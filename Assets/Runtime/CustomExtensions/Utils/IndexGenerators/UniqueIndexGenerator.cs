using Core.Utils.Extensions;

namespace Core.Utils.IndexGenerators
{
    public class UniqueIndexGenerator : IIndexGenerator
    {
        private readonly int _maxValue;
        private readonly bool _shuffleOnEnd;

        private int _currentObjectIndex = -1;
        private int[] _indexes;

        public UniqueIndexGenerator(int maxValue, bool shuffleOnEnd = false)
        {
            _maxValue = maxValue;
            _shuffleOnEnd = shuffleOnEnd;

            Reset();
        }

        public int Get()
        {
            _currentObjectIndex++;

            if (_currentObjectIndex >= _indexes.Length)
            {
                _currentObjectIndex = 0;

                if (_shuffleOnEnd)
                    _indexes.Shuffle();
            }

            return _indexes[_currentObjectIndex];
        }

        public void Reset()
        {
            _indexes = new int[_maxValue];

            for (int i = 0; i < _maxValue; i++)
            {
                _indexes[i] = i;
            }

            _indexes.Shuffle();
        }
    }
}