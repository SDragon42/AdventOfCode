using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.CSharp.Common
{
    public static class PuzzleRunner
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static IPuzzle CreatePuzzle(int? day)
        {
            Type puzzle = GetPuzzleType(day);
            return (IPuzzle)Activator.CreateInstance(puzzle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        private static Type GetPuzzleType(int? day)
        {
            var puzzleTypes = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => typeof(IPuzzle).IsAssignableFrom(t));

            // Get the specified day Puzzle
            if (day.HasValue)
                return puzzleTypes
                    .Where(p => p.Name == $"Day{day:00}")
                    .FirstOrDefault();

            // Get the highest day puzzle
            return puzzleTypes
                .OrderByDescending(t => t.Name)
                .FirstOrDefault();
        }
    }
}
