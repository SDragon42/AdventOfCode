using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AdventOfCode.CSharp.Common;

namespace AdventOfCode.CSharp.Year2019
{
    class Program
    {
        static void Main(string[] args)
        {
            //var firstArg = args.FirstOrDefault();

            //var asm = Assembly.GetExecutingAssembly();
            //var allPuzzleTypes = asm.GetTypes()
            //    .Where(t => typeof(IPuzzle).IsAssignableFrom(t))
            //    .Where(t => !t.IsInterface)
            //    .Where(t => !t.IsAbstract)
            //    .OrderBy(t => t.Name)
            //    .ToList();

            //IEnumerable<Type> typeList = null;
            //if (firstArg != null)
            //    typeList = allPuzzleTypes.Where(t => t.Name.StartsWith(firstArg));

            //if (typeList == null)
            //    typeList = allPuzzleTypes.Where(t => t.Equals(allPuzzleTypes.LastOrDefault())); // Run last puzzle

            //// Run all puzzles
            //typeList.ForEach(RunPuzzle);
            var pz = PuzzleRunner.GetPuzzle(1);
            if (pz != null)
                pz.Run();
        }

        static void RunPuzzle(Type typeItem)
        {
            var puzzle = (IPuzzle)Activator.CreateInstance(typeItem);
            if (puzzle == null)
                return;
            Line();
            var sw = new Stopwatch();
            sw.Start();
            puzzle.Run();
            sw.Stop();
            Console.WriteLine($"BENCHMARK: {sw.ElapsedMilliseconds} ms");
        }

        static void Line() => Console.WriteLine(string.Empty.PadRight(60, '-'));

    }
}
