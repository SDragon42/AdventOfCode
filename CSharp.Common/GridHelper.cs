using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AdventOfCode.CSharp.Common
{
    public static class GridHelper
    {
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


        public static string DrawScreenGrid2D<TValue>(IList<IList<TValue>> gridData, Func<TValue, string> DrawElementMethod = null)
        {
            var minX = 0;
            var maxX = gridData[0].Count - 1;
            var minY = 0;
            var maxY = gridData.Count - 1;

            var text = new StringBuilder();
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var value = gridData[y][x];

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
