using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advent_of_Code
{
    static class Helper
    {
        /// <summary>
        /// Gets a single digit from the specified number (right to left).
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <param name="position">The digit position, from right to left.</param>
        /// <returns></returns>
        public static int GetDigitRight(int value, int position)
        {
            var modifier = Convert.ToInt32(Math.Pow(10, position - 1));
            var result = (value / modifier) % 10;
            return result;
            //if (value == 0)
            //    return 0;
            //var numDigits = Convert.ToInt32(Math.Floor(Math.Log10(value) + 1));
            //if (position < 1)
            //    position = 1;
            //var offset = numDigits - position + 1;
            //var result = Math.Truncate(value / Math.Pow(10, numDigits - offset))
            //          - (Math.Truncate(value / Math.Pow(10, numDigits - offset + 1)) * 10);
            //return Convert.ToInt32(result);
        }
        /// <summary>
        /// Gets a single digit from the specified number (right to left).
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <param name="position">The digit position, from right to left.</param>
        /// <returns></returns>
        public static int GetDigitRight(long value, int position)
        {
            var modifier = Convert.ToInt32(Math.Pow(10, position - 1));
            var result = (value / modifier) % 10;
            return (int)result;
        }

        /// <summary>
        /// Gets a single digit from the specified number (left to right).
        /// </summary>
        /// <param name="value">The number value.</param>
        /// <param name="position">The digit position, from left to right.</param>
        /// <returns></returns>
        public static int GetDigitLeft(int value, int position)
        {
            var result = Math.Truncate(value / Math.Pow(10, 6 - position)) - (Math.Truncate(value / Math.Pow(10, 6 - position + 1)) * 10);
            return Convert.ToInt32(result);
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
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items)
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

        public static string GetFileContent(string filename)
        {
            var fullPath = Path.Combine(@"..\..\..\Data", filename);
            var content = File.ReadAllText(fullPath);
            return content;
        }

        public static string[] GetFileContentAsLines(string filename)
        {
            var fullPath = Path.Combine(@"..\..\..\Data", filename);
            var lines = File.ReadAllLines(fullPath);
            return lines;
        }

    }
}
