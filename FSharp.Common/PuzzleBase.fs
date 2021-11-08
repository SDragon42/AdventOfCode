namespace FSharp.Common

open System
open System.Collections.Generic
open System.Diagnostics
open System.Linq
open System.Reflection

[<AbstractClass>]
type PuzzleBase (runBenchmarks:bool, runExamples:bool) =

    let benchmark: bool = runBenchmarks
    let examples: bool = runExamples


    member this.RunWithoutBenchmarks (method : unit -> string) =
        method()


    member this.RunWithBenchmarks (method : unit -> string) =
        let benchmark = new Stopwatch()
        benchmark.Start()
        let text = method()
        benchmark.Stop()
        text + $" - {benchmark.ElapsedMilliseconds} ms"


    member this.RunDoNothing (method : unit -> string) =
        null

    

    member this.Run (method : unit -> string) =
        if benchmark then
            this.RunWithBenchmarks(method)
        else
            this.RunWithoutBenchmarks(method)
        
        

    member this.RunExample (method : unit -> string) =
        if (runExamples) then
            this.Run(method)
        else
            this.RunDoNothing(method)
            

    abstract member SolvePuzzle: _ -> IEnumerable<String>
