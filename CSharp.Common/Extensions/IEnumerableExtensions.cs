using System;
using System.Collections.Generic;

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


    }
}
