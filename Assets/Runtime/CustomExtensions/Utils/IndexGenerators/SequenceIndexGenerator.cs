namespace Core.Utils.IndexGenerators
{
    public class SequenceIndexGenerator : IIndexGenerator
    {
        private readonly int _maxValue;
        private int _currentIndex = -1;

        public SequenceIndexGenerator(int maxValue)
        {
            _maxValue = maxValue;
        }

        public int Get()
        {
            _currentIndex++;

            if (_currentIndex >= _maxValue)
                _currentIndex = 0;

            return _currentIndex;
        }

        public void Reset()
        {
            _currentIndex = 0;
        }
    }
}