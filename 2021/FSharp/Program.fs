open System
open FSharp.Common

[<EntryPoint>]
let main args =

    let runner = PuzzleRunner([|
        "Advent of Code 2021";
        "https://adventofcode.com/2021";
        "By: SDragon"|])
    
    runner.Run(args)

    0 // return an integer exit code
