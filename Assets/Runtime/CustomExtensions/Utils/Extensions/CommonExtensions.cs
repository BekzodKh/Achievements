using System.Linq;

namespace Core.Utils.Extensions
{
    public static class CommonExtensions
    {
        //note: very useful for validating Enums
        public static bool IsOneOf<T>(this T item, params T[] options)
        {
            return options.Contains(item);
        }
    }
}