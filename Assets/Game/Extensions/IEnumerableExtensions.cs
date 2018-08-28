using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Game.Extensions
{
    public static class IEnumerableExtensions
    {
        public static T TakeRandom<T>(this IEnumerable<T> source)
        {
            var availableCount = source.Count();
            if (availableCount == 0) { throw new Exception("Unable to pick random number from empty list"); }

            var random = Random.Range(0, availableCount);
            return source.ElementAt(random);
        }

        public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> source, int count)
        {
            var availableCount = source.Count();
            if (availableCount == 0) { throw new Exception("Unable to pick random number from empty list"); }
            if (availableCount == count) { return source; } // Pointless if anyone ever did this but just incase

            var randomElements = new List<T>();
            for (var i = 0; i < count; i++)
            {
                var random = Random.Range(0, availableCount);
                var randomElement = source.ElementAt(random);
                if (randomElements.Contains(randomElement))
                {
                    i--;
                    continue;
                }
                randomElements.Add(randomElement);
            }
            return randomElements;
        }
    }
}