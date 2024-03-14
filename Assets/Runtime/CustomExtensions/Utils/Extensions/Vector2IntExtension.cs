using UnityEngine;

namespace Core.Utils.Extensions
{
    public static class Vector2IntExtension
    {
        /// <summary>
        ///     Random int value between x and y inclusive
        /// </summary>
        public static int Random(this Vector2Int vector2Int)
        {
            return UnityEngine.Random.Range(vector2Int.x, vector2Int.y + 1);
        }
    }
}