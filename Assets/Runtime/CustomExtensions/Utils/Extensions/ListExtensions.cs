using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;

namespace Core.Utils.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                var temp = array[i];
                int random = Random.Range(i, array.Count);
                array[i] = array[random];
                array[random] = temp;
            }
        }

        public static T GetRandomElementWithDeletion<T>(this List<T> sourceList)
        {
            var randomElement = sourceList.GetRandomElement();

            sourceList.Remove(randomElement);

            return randomElement;
        }
        
        public static int Replace<T>(this List<T> source, T oldValue, T newValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var index = source.IndexOf(oldValue);
            
            if (index != -1)
            {
                source[index] = newValue;
            }
            
            return index;
        }

        public static void ReplaceAll<T>(this List<T> source, T oldValue, T newValue)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int index;
            do
            {
                index = source.IndexOf(oldValue);
                
                if (index != -1)
                {
                    source[index] = newValue;
                }
            } while (index != -1);
        }
    }
}