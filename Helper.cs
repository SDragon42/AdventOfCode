using System;
using System.Collections.Generic;
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
            if (value == 0)
                return 0;
            var numDigits = Convert.ToInt32(Math.Floor(Math.Log10(value) + 1));
            if (position < 1)
                position = 1;
            var offset = numDigits - position + 1;
            var result = Math.Truncate(value / Math.Pow(10, numDigits - offset))
                      - (Math.Truncate(value / Math.Pow(10, numDigits - offset + 1)) * 10);
            return Convert.ToInt32(result);
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
    }
}
