using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace Advent_of_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            var asm = Assembly.GetExecutingAssembly();
            var allPuzzleTypes = asm.GetTypes()
                .Where(t => typeof(IPuzzle).IsAssignableFrom(t))
                .Where(t => !t.IsInterface)
                .Where(t => !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToList();

            var typeList = allPuzzleTypes
                .Where(t => t.Name.StartsWith("Day07Puzzle2"))
                //.Where(t => t.Equals(allPuzzleTypes.LastOrDefault())) // Run last puzzle
                ;

            // Run all puzzles
            typeList.ForEach(RunPuzzle);
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
