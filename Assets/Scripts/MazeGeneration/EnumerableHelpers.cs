using System.Collections.Generic;
using System.Linq;

namespace ShareefSoftware
{
    public static class EnumerableHelpers
    {
        /// Enumerates a list in a random order.
        public static IEnumerable<T> Shuffle<T>(IList<T> finiteList, System.Random random = null)
        {
            random ??= new System.Random();

            int count = finiteList.Count;
            var permutation = new int[count];
            for (int i = 0; i < count; i++)
            {
                int index = random.Next(i + 1);
                if (index != i)
                    permutation[i] = permutation[index];
                permutation[index] = i;
            }

            for (int j = 0; j < count; j++)
            {
                yield return finiteList[permutation[j]];
            }
        }
    }
}