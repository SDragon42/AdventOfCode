namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



type private Puzzle01 () =

    member private this.CountIncreases = 
        List.pairwise 
        >> List.filter (fun (a,b) -> a < b) 
        >> List.length


    member this.RunPart1 (input:int list) =
        // How many measurements are larger than the previous measurement?
        let result = 
            input
            |> this.CountIncreases
        result


    member this.RunPart2 (input:int list) =
        // How many sums are larger than the previous sum?
        let result =
            input
            |> List.windowed(3)
            |> List.map List.sum
            |> this.CountIncreases
        result



module Day01 =
    let private GetPuzzleInput (part:int) (name:string) =
        let day = 1

        let input = 
            InputHelper.LoadLines (day, name)
            |> Seq.map int
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

        let actual = (new Puzzle01()).RunPart1 input

        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)


    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name

        let actual = (new Puzzle01()).RunPart2 input

        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
