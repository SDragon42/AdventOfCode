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
            var tl = asm.GetTypes()
                .Where(t => typeof(IPuzzle).IsAssignableFrom(t))
                .Where(t => !t.IsInterface)
                .Where(t => !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToList();

            foreach (var item in tl)
            {
                var puzzle = (IPuzzle)Activator.CreateInstance(item);
                if (puzzle == null)
                    continue;
                Line();
                puzzle?.Run();
            }
            
        }

        static void Line () => Console.WriteLine(string.Empty.PadRight(60, '-'));
    }
}
