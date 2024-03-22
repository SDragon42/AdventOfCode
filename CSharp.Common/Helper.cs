using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode.CSharp.Common
{
    public static class Helper
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
            var digit = value.ToString()[position - 1];
            var result = digit - 48; // ASCII number offset
            return result;
        }
        //public static int GetDigitLeft(int value, int position)
        //{
        //    const int MaxPosition = 6;
        //    var result = Math.Truncate(value / Math.Pow(10, MaxPosition - position))
        //        - (Math.Truncate(value / Math.Pow(10, MaxPosition - position + 1)) * 10);
        //    return Convert.ToInt32(result);
        //}


        /// <summary>
        /// Finds the Greatest Common Factor between two numbers.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://stackoverflow.com/a/20824923/6136
        /// </remarks>
        public static long FindGreatestCommonFactor(long a, long b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Find the Least Common Multiple of two numbers.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://stackoverflow.com/a/20824923/6136
        /// </remarks>
        public static long FindLeastCommonMultiple(long a, long b)
        {
            return (a / FindGreatestCommonFactor(a, b)) * b;
        }



        public static string DrawPointGrid2D<TValue>(IDictionary<Point, TValue> gridData, Func<TValue, string> DrawElementMethod = null, Size? minArea = null)
        {
            var minX = gridData.Keys.Select(h => h.X).Min();
            var maxX = gridData.Keys.Select(h => h.X).Max();
            var minY = gridData.Keys.Select(h => h.Y).Min();
            var maxY = gridData.Keys.Select(h => h.Y).Max();

            if (minArea.HasValue)
            {
                var xDiff = minArea.Value.Width - Math.Abs(maxX - minX);
                var yDiff = minArea.Value.Height - Math.Abs(maxY - minY);

                if (xDiff > 0)
                {
                    var xPart = xDiff / 2;
                    minX -= xPart;
                    maxX += xDiff = xPart;
                }

                if (yDiff > 0)
                {
                    var yPart = yDiff / 2;
                    minY -= yPart;
                    maxY += yDiff = yPart;
                }
            }

            var text = new StringBuilder();
            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var key = new Point(x, y);
                    var value = default(TValue);
                    if (gridData.ContainsKey(key))
                        value = gridData[key];

                    var element = DrawElementMethod?.Invoke(value) ?? value?.ToString() ?? " ";
                    text.Append(element);
                }
                if (y > minY)
                    text.AppendLine();
            }

            var result = text.ToString();
            return result;
        }

        public static string DrawScreenGrid2D<TValue>(IDictionary<Point, TValue> gridData, Func<TValue, string> DrawElementMethod = null)
        {
            var minX = gridData.Keys.Select(h => h.X).Min();
            var maxX = gridData.Keys.Select(h => h.X).Max();
            var minY = gridData.Keys.Select(h => h.Y).Min();
            var maxY = gridData.Keys.Select(h => h.Y).Max();

            var text = new StringBuilder();
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var key = new Point(x, y);
                    var value = default(TValue);
                    if (gridData.ContainsKey(key))
                        value = gridData[key];

                    var element = DrawElementMethod?.Invoke(value) ?? value.ToString();
                    text.Append(element);
                }
                text.AppendLine();
            }

            return text.ToString();
        }

        /// <summary>
        /// Converts an array index to a grid x,y point.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="xMax"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Point IndexToPoint(int index, int xMax, Point? origin = null)
        {
            var x = (index % xMax) + (origin?.X ?? 0);
            var y = (index / xMax) + (origin?.Y ?? 0);

            return new Point(x, y);
        }
    }
}
