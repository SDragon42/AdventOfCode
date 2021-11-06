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
            //var xxx = new Day15_Old.Day15Puzzle1();
            //xxx.Run();

            var runner = new PuzzleRunner(
                "Advent of Code 2019",
                "https://adventofcode.com/2019",
                "By: SDragon");
            runner.Run(args);
        }

    }
}
