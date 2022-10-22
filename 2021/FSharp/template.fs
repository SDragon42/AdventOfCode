﻿namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



type private Puzzle () =

    member private this.CountIncreases = 
        List.pairwise 
        >> List.filter (fun (a,b) -> a < b) 
        >> List.length


    member this.RunPart1 (input:string list) =
        // <Puzzle Question>
        let result = 0
        result


    member this.RunPart2 (input:string list) =
        // <Puzzle Question>
        let result = 0
        result



module Template =
    let private GetPuzzleInput (part:int) (name:string) =
        let day = 0

        let input = 
            InputHelper.LoadLines (day, name)
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt
    
        input, answer


    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let input, expected = GetPuzzleInput 1 name

        let actual = (new Puzzle()).RunPart1 (input)

        match expected with
        | None -> Assert.Null(actual)
        | _ -> Assert.Equal(expected.Value, actual)


    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name

        let actual = (new Puzzle()).RunPart2 (input)

        match expected with
        | None -> Assert.Null(actual)
        | _ -> Assert.Equal(expected.Value, actual)
