using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.CSharp.Common
{
    public static class InputHelper
    {
        /// <summary>
        /// Builds the path for an input data file.
        /// </summary>
        /// <param name="day">The puzzle day for the data files.</param>
        /// <param name="name">The name of the data file without extension.</param>
        /// <param name="rootPath">The root path.</param>
        /// <returns></returns>
        private static string BuildDataFilePath(int day, string name, string rootPath = ".")
        {
            var filename = $@"{rootPath}\input\Day{day:00}\{name}.txt";
            return filename;
        }

        /// <summary>
        /// Reads all lines from the input file.
        /// </summary>
        /// <param name="day">The puzzle day for the data files.</param>
        /// <param name="name">The name of the data file without extension.</param>
        /// <param name="rootPath">The root path.</param>
        /// <returns></returns>
        public static IEnumerable<string> ReadLines(int day, string name, string rootPath = ".")
        {
            try
            {
                var path = BuildDataFilePath(day, name, rootPath);
                return File.ReadLines(path);
            }
            catch { return null; }
        }

        /// <summary>
        /// Reads all text from the input file.
        /// </summary>
        /// <param name="day">The puzzle day for the data files.</param>
        /// <param name="name">The name of the data file without extension.</param>
        /// <param name="rootPath">The root path.</param>
        /// <returns></returns>
        public static string ReadText(int day, string name, string rootPath = ".")
        {
            var lines = ReadLines(day, name, rootPath);
            if (lines == null)
                return null;

            var text = string.Join(Environment.NewLine, lines);
            return text?.Length > 0 ? text : null;
        }

    }
}
