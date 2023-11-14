using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.CSharp.Common
{
    public static class InputHelper
    {
        [Obsolete("Replace with: ReadLines(day, name)")]
        public static IEnumerable<string> LoadInputFile(int day, string name)
        {
            var filename = GetDataFilePath(day, name);
            return File.ReadLines(filename);
        }

        [Obsolete("Replace with: ReadLines(day, $\"{name}-answer{part}\")")]
        public static IEnumerable<string> LoadAnswerFile(int day, int part, string name)
        {
            try
            {
                var filename = GetDataFilePath(day, $"{name}-answer{part}");
                return File.ReadLines(filename);
            }
            catch { return null; }
        }


        /// <summary>
        /// Creates a 
        /// </summary>
        /// <param name="day">The puzzle day for the data files.</param>
        /// <param name="name">The name of the data file without extension.</param>
        /// <returns></returns>
        private static string GetDataFilePath(int day, string name)
        {
            var filename = $@".\input\Day{day:00}\{name}.txt";
            return filename;
        }

        /// <summary>
        /// Reads all lines from the input file.
        /// </summary>
        /// <param name="day">The puzzle day for the data files.</param>
        /// <param name="name">The name of the data file without extension.</param>
        /// <returns></returns>
        public static IEnumerable<string> ReadLines(int day, string name)
        {
            try
            {
                var path = GetDataFilePath(day, name);
                return File.ReadLines(path);
            }
            catch { return null; }
        }

        /// <summary>
        /// Reads all text from the input file.
        /// </summary>
        /// <param name="day">The puzzle day for the data files.</param>
        /// <param name="name">The name of the data file without extension.</param>
        /// <returns></returns>
        public static string ReadText(int day, string name)
        {
            var lines = ReadLines(day, name);
            if (lines == null)
                return null;

            var text = string.Join(Environment.NewLine, lines);
            return text?.Length > 0 ? text : null;
        }

    }
}
