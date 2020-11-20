using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent_of_Code
{
    static class Extensions
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
    }
}
