namespace Core.Factories
{
    public class ObjectFactory<T> : IFactory<T>
        where T : class, new()
    {
        public T Create()
        {
            return new T();
        }
    }
}