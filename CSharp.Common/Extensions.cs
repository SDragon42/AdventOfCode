using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.CSharp.Common
{
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <param name="actionMethod"></param>
        public static void ForEach<T>(this IEnumerable<T> elements, Action<T> actionMethod)
        {
            if (actionMethod == null)
                return;
            foreach (var item in elements)
                actionMethod(item);
        }


        public static int ToInt32(this string text) => (int)Convert.ChangeType(text, typeof(int));
        public static long ToInt64(this string text) => (long)Convert.ChangeType(text, typeof(long));

    }
}
