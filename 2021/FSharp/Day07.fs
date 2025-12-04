namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 07: The Treachery of Whales`` =
    let day = 7



    //-------------------------------------------------------------------------



    type private Puzzle () =

        member private this.CalcMedian (data: int list) =
            let CalcMedianOdd (data: int list) =
                let p = data.Length + 1 / 2
                data[p - 1]
            let CalcMedianEven (data: int list) =
                let p = data.Length / 2
                (data[p - 1] + data[p]) / 2

            let orderedData = data |> List.sortBy (fun t -> t)

            match data.Length % 2 with
            | 0 -> CalcMedianEven orderedData
            | _ -> CalcMedianOdd orderedData

    
        member private this.CalcMeans (data: int list) =
            let sum = data |> List.sum
            let mean = sum / data.Length
            [mean - 1; mean; mean + 1]

    
        member private this.CalcFuel1(position: int, target: int) =
            let fuel = abs(position - target)
            fuel


        member private this.CalcFuel2(position: int, target: int) =
            let distance = abs(position - target)
            let fuel = (distance * (distance + 1)) / 2
            fuel


        // How much fuel must they spend to align to that position?
        member this.RunPart1 (input: int list) =
            let median = this.CalcMedian(input)

            let result =
                input
                |> List.map (fun h -> this.CalcFuel1(h, median))
                |> List.sum
            result


        // How much fuel must they spend to align to that position?
        member this.RunPart2 (input: int list) =
            let DoIt (target: int) = 
                input 
                |> List.map (fun h -> this.CalcFuel2(h, target))
                |> List.sum

            let result = 
                this.CalcMeans(input)
                |> List.map DoIt 
                |> List.min
            result

        

    //-------------------------------------------------------------------------


        
    let private GetPuzzleInput (part:int) (name:string) =
        let input =
            InputHelper.LoadText(2021, day, name).Split(',')
            |> Array.toList
            |> List.map int
                
        let answer = 
            InputHelper.LoadAnswer (2021, day, $"%s{name}-answer%i{part}")
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
