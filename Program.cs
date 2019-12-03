using System;
using System.Reflection;
using System.Linq;

namespace Advent_of_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            var asm = Assembly.GetExecutingAssembly();
            var typeList = asm.GetTypes()
                .Where(t => typeof(IPuzzle).IsAssignableFrom(t))
                .Where(t => !t.IsInterface)
                .Where(t => !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToList();

            //typeList.ForEach(RunPuzzle);
            RunPuzzle(typeList.LastOrDefault());
        }

        static void RunPuzzle(Type typeItem)
        {
            var puzzle = (IPuzzle)Activator.CreateInstance(typeItem);
            if (puzzle == null)
                return;
            Line();
            puzzle.Run();
        }

        static void Line() => Console.WriteLine(string.Empty.PadRight(60, '-'));
    }
}
