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

        let rec GetTime(time:double, units: string) =
            match units with
            | "ms" ->
                    if time < 1000
                    then $"%.0f{time}", units
                    else GetTime(time / 1000.0, "sec")
            | "sec" ->
                    if time < 60
                    then $"%.1f{time}", units
                    else GetTime(time / 60.0, "min  :(")
            | _ ->
                    $"%.1f{time}", units

        let time, units = GetTime(benchmark.ElapsedMilliseconds |> double, "ms")
        text + $"   {time} {units}"


    member this.RunDoNothing (method : unit -> string) =
        null

    

    member this.RunProblem (method : unit -> string) =
        if benchmark then
            this.RunWithBenchmarks(method)
        else
            this.RunWithoutBenchmarks(method)
        
        

    member this.RunExample (method : unit -> string) =
        if (runExamples) then
            this.RunProblem(method)
        else
            this.RunDoNothing(method)
            

    abstract member SolvePuzzle: _ -> IEnumerable<String>
