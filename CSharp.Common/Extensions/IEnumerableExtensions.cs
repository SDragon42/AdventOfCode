using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.CSharp.Common
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <param name="actionMethod"></param>
        public static void ForEach<T>(this IEnumerable<T> elements, Action<T> actionMethod)
        {
            if (elements is null) throw new ArgumentNullException(nameof(elements));
            if (actionMethod is null) throw new ArgumentNullException(nameof(actionMethod));

            foreach (var item in elements)
                actionMethod(item);
        }


        /// <summary>
        /// Returns an enumerable yielding a sliding window containing elements drawn from the input enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        /// <remarks>Code from: https://stackoverflow.com/a/8877876/6136</remarks>
        public static IEnumerable<T[]> Windowed<T>(this IEnumerable<T> elements, int windowSize)
        {
            if (elements is null) throw new ArgumentNullException(nameof(elements));
            if (windowSize <= 0) throw new ArgumentOutOfRangeException(nameof(windowSize));

            using (var e = elements.GetEnumerator())
            {
                var arr = new T[windowSize];

                var i = 0;
                var r = windowSize - 1;

                while (e.MoveNext())
                {
                    arr[i] = e.Current;
                    i = (i + 1) % windowSize;

                    if (r == 0)
                        yield return CreateWindowArray(j => arr[(i + j) % windowSize]);
                    else
                        r--;
                }
            }


            T[] CreateWindowArray(Func<int, T> GetValue)
            {
                var output = new T[windowSize];
                for (var i = 0; i < windowSize; i++)
                    output[i] = GetValue(i);
                return output;
            }
        }


        /// <summary>
        /// Returns a list of all possible combinations of the item list.
        /// (item list only tested with unique values)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sourced and modified from:
        /// https://stackoverflow.com/questions/5132758/words-combinations-without-repetition
        /// </remarks>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> items)
        {
            if (items.Count() == 1)
            {
                yield return new T[] { items.First() };
                yield break;
            }

            foreach (var item in items)
            {
                var nextItems = items.Where(i => !i.Equals(item));
                foreach (var result in GetPermutations(nextItems))
                    yield return new T[] { item }.Concat(result);
            }
        }
    }
}
