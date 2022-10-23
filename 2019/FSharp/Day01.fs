namespace AdventOfCode.FSharp.Year2019

open System
open Xunit
open FSharp.Common



module ``Day 1: The Tyranny of the Rocket Equation`` =
    let day = 1



    //-------------------------------------------------------------------------



    type Puzzle () =

        member private this.CalcFuel mass : int =
            (mass / 3) - 2


        member private this.CalcFuelCorrectly mass : int =
            let fuelMass = this.CalcFuel(mass)
            match fuelMass with
            | _ when fuelMass > 0 -> fuelMass + this.CalcFuelCorrectly(fuelMass)
            | _ -> 0


        // What is the sum of the fuel requirements?
        member this.RunPart1 (input:int list) =
            let totalFuel = input |> List.sumBy this.CalcFuel
            totalFuel


        // What is the sum of the fuel requirements?
        member this.RunPart2 (input:int list) =
            let totalFuel = input |> List.sumBy this.CalcFuelCorrectly
            totalFuel



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =
        let input = 
            InputHelper.LoadLines(day, name)
            |> Seq.map int
            |> Seq.toList
        
        let answer =
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        input, answer



    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("example2")>]
    [<InlineData("example3")>]
    [<InlineData("example4")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let input, expected = GetPuzzleInput 1 name

        let actual = (new Puzzle()).RunPart1 input

        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)


    [<Theory>]
    [<InlineData("example2")>]
    [<InlineData("example3")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name

        let actual = (new Puzzle()).RunPart2 input

        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
