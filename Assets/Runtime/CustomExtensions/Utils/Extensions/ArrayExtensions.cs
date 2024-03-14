using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;

namespace Core.Utils.Extensions
{
    public static class ArrayExtensions
    {
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                T temp = array[i];
                int random = Random.Range(i, array.Length);
                array[i] = array[random];
                array[random] = temp;
            }
        }
        
        public static void AddDaysOfWeek(this Dictionary<DayOfWeek, bool> targetDays)
        {
            targetDays.Add(DayOfWeek.Monday, true);
            targetDays.Add(DayOfWeek.Tuesday, true);
            targetDays.Add(DayOfWeek.Wednesday, true);
            targetDays.Add(DayOfWeek.Thursday, true);
            targetDays.Add(DayOfWeek.Friday, true);
            targetDays.Add(DayOfWeek.Saturday, true);
            targetDays.Add(DayOfWeek.Sunday, true);
        }
    }
}