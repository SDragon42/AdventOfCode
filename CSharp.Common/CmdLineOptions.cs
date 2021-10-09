﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

namespace AdventOfCode.CSharp.Common
{
    class CmdLineOptions
    {
        [Option('b', "benchmark", HelpText = "Benchmark puzzles")]
        public bool Benchmark { get; set; }

        [Option('a', "all", HelpText = "Run all puzzles")]
        public bool RunAll { get; set; }

        [Value(0)]
        public IEnumerable<int> PuzzleDays { get; set; }
    }
}