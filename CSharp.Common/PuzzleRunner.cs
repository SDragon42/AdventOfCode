using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.CSharp.Common
{
    public class PuzzleRunner
    {
        private readonly string[] titleLines;



        /// <summary>
        /// </summary>
        /// <param name="titles">The title lines to write at the start.</param>
        public PuzzleRunner(params string[] titles)
        {
            this.titleLines = titles;
        }



        /// <summary>
        /// Runs all the puzzles specified by command line arguments.
        /// </summary>
        /// <param name="args">The command line arguments</param>
        public void Run(string[] args)
        {
            WriteHeader();

            Parser.Default
                .ParseArguments<CmdLineOptions>(args)
                .WithParsed(SolvePuzzles);
        }



        void SolvePuzzles(CmdLineOptions options)
        {
            var puzzleTypes = GetPuzzleTypes(options).ToList();
            for (var i = 0; i < puzzleTypes.Count; i++)
            {
                var puzzle = (PuzzleBase)Activator.CreateInstance(puzzleTypes[i]);
                puzzle.SetOptions(options.RunBenchmark, options.RunExamples);

                puzzle.SolvePuzzle()
                    .Where(t => t != null)
                    .ForEach(Console.WriteLine);

                WriteSectionSeperator();
            }
        }

        IEnumerable<Type> GetPuzzleTypes(CmdLineOptions options)
        {
            var puzzleTypes = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(t => typeof(PuzzleBase).IsAssignableFrom(t));

            // All puzzles?
            if (options.RunAll)
                return puzzleTypes;

            // only the last puzzle?
            if (options.PuzzleDays.Count() == 0)
                return puzzleTypes
                    .OrderByDescending(t => t.Name)
                    .Take(1);

            // all specified puzzles
            var puzzleNames = options.PuzzleDays.Select(d => $"Day{d:00}").ToList();
            return puzzleTypes
                .Where(t => puzzleNames.Any(p => t.Name.StartsWith(p)));
        }

        void WriteHeader()
        {
            var titleWidth = titleLines.Select(t => t.Length).Max();
            var titleBorder = "+-" + string.Empty.PadRight(titleWidth, '-') + "-+";

            Console.WriteLine(titleBorder);
            foreach (var text in titleLines)
                Console.WriteLine($"| {text.PadRight(titleWidth)} |");
            Console.WriteLine(titleBorder);
            Console.WriteLine();
        }

        void WriteSectionSeperator()
        {
            Console.WriteLine();
            Console.WriteLine(string.Empty.PadRight(60, '-'));
            Console.WriteLine();
        }

    }

}
