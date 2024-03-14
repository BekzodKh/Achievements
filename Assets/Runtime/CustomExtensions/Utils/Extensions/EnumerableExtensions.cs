using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            return source.Shuffle().First();
        }

        public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}