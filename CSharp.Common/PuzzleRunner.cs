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

        public static IPuzzle GetPuzzle(int day)
        {
            var puzzleType = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(p => p.Name == $"Day{day:00}")
                .FirstOrDefault();
            return (IPuzzle)Activator.CreateInstance(puzzleType);
        }
    }
}
