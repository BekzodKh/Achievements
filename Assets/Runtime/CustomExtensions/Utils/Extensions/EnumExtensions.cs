using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Utils.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetCastedFlags<T>(this Enum e) where T : Enum
        {
            return GetFlags(e).Cast<T>();
        }

        public static IEnumerable<Enum> GetFlags(this Enum e)
        {
            return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag);
        }
    }
}