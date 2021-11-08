// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open FSharp.Common

[<EntryPoint>]
let main args =

    let runner = PuzzleRunner([|
        "Advent of Code 2019";
        "https://adventofcode.com/2019";
        "By: SDragon"|])
    
    runner.Run(args)

    0 // return an integer exit code
