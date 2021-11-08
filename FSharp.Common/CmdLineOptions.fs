namespace FSharp.Common

open System.Collections.Generic
open CommandLine

type CmdLineOptions() =
    
    [<Option('b', "benchmarks", HelpText = "Benchmark puzzles")>]
    //member this.RunBenchmark: bool = false
    member val RunBenchmark: bool = false with get, set

    [<Option('e',"examples", HelpText = "Run the example inputs")>]
    //member this.RunExamples: bool = false
    member val RunExamples: bool = false with get, set

    [<Option('a', "all", HelpText = "Run all puzzles")>]
    //member this.RunAll: bool = false
    member val RunAll: bool = false with get, set

    [<Value(0)>]
    //member this.PuzzleDays: IEnumerable<int> = null
    member val PuzzleDays: IEnumerable<int> = null with get, set
