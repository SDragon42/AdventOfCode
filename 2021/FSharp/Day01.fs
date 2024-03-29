﻿namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 01: Sonar Sweep`` =
    let day = 1



    //-------------------------------------------------------------------------



    type private Puzzle () =

        member private this.CountIncreases = 
            List.pairwise 
            >> List.filter (fun (a,b) -> a < b) 
            >> List.length


        // How many measurements are larger than the previous measurement?
        //member this.RunPart1 (input:int list) =
        //    input
        //    |> this.CountIncreases

        // How many measurements are larger than the previous measurement?
        member this.RunPart1 =
            this.CountIncreases


        // How many sums are larger than the previous sum?
        //member this.RunPart2 (input:int list) =
        //    input
        //    |> List.windowed(3)
        //    |> List.map List.sum
        //    |> this.CountIncreases

        // How many sums are larger than the previous sum?
        member this.RunPart2 =
            List.windowed(3)
            >> List.map List.sum
            >> this.CountIncreases



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =
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

        let actual = (new Puzzle()).RunPart1 input

        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)


    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name

        let actual = (new Puzzle()).RunPart2 input

        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
