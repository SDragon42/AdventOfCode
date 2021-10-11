using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AdventOfCode.CSharp.Common
{
    public static class InputHelper
    {
        public static IEnumerable<string> LoadInputFile(int day, string name)
        {
            var filename = $@".\input\Day{day:00}\{name}.txt";
            return File.ReadLines(filename);
        }
        public static IEnumerable<string> LoadAnswerFile(int day, int part, string name)
        {
            try
            {
                var filename = $@".\input\Day{day:00}\{name}-answer{part}.txt";
                return File.ReadLines(filename);
            }
            catch { return null; }
        }


        #region Old Code
        ///// <summary>Returns the file as a single string.
        ///// </summary>
        ///// <param name="name">The name of the data file to load.</param>
        ///// <returns></returns>
        //public static string LoadText(int day, string name)
        //{
        //    var filename = GenerateFilename(name);
        //    var result = File.ReadAllText(filename);
        //    return result;
        //}

        ///// <summary>Returns each line from the file as an enumeration of type T.
        ///// </summary>
        ///// <param name="name">The name of the data file to load.</param>
        ///// <typeparam name="T">The data type to convert each line to.</typeparam>
        ///// <returns></returns>
        //public static IEnumerable<T> LoadLines<T>(string name)
        //{
        //    var filename = GenerateFilename(name);
        //    var result = File.ReadLines(filename)
        //        .Select(l => l.Trim())
        //        .Where(l => l.Length > 0)
        //        .Select(l => (T)Convert.ChangeType(l, typeof(T)));
        //    return result;
        //}

        ///// <summary>Returns each line from the file as an enumeration of arrays of type T.
        ///// </summary>
        ///// <param name="name">The name of the data file to load.</param>
        ///// <typeparam name="T">The data type to convert each element of the array to.</typeparam>
        ///// <returns></returns>
        //public static IEnumerable<T[]> LoadLinesAsCSV<T>(string name)
        //{
        //    var lines = LoadLines<string>(name);
        //    foreach (var line in lines)
        //    {
        //        var result = line
        //            .Split(',')
        //            .Select(i => (T)Convert.ChangeType(i, typeof(T)))
        //            .ToArray();
        //        yield return result;
        //    }
        //}

        ///// <summary>Generates the standard path to a data file.
        ///// </summary>
        ///// <param name="name">The name of the data file.</param>
        ///// <returns></returns>
        //static string GenerateFilename(string name)
        //{
        //    var filename = $@".\input\{name}.txt";
        //    return filename;
        //}

        #endregion

    }
}
