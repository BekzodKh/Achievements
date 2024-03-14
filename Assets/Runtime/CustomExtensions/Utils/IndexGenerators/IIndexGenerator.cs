namespace Core.Utils.IndexGenerators
{
    public interface IIndexGenerator
    {
        int Get();

        void Reset();
    }
}