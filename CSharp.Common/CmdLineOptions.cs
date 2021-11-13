namespace AdventOfCode.CSharp.Common;

class CmdLineOptions
{
    [Option('b', "benchmarks", HelpText = "Benchmark puzzles")]
    public bool RunBenchmark { get; set; }

    [Option('e', "examples", HelpText = "Run the example inputs")]
    public bool RunExamples { get; set; }

    [Option('a', "all", HelpText = "Run all puzzles")]
    public bool RunAll { get; set; }

    [Value(0)]
    public IEnumerable<int> PuzzleDays { get; set; }
}
