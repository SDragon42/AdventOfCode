namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 03: Binary Diagnostic`` =
    let day = 3



    //-------------------------------------------------------------------------



    type private Puzzle () =

        member private this.GetdigitsAt (position: int, input: string list) =
            let GetDigitAt(value: string) =
                value[position]
            input |> List.map GetDigitAt


        member private this.MostCommonBit (bits: char list) =
            let a = this.CountChars bits '0'
            let b = this.CountChars bits '1'
            match a with
            | _ when a <= b -> '1'
            | _ -> '0'


        member private this.LeastCommonBit (bits: char list) =
            let a = this.CountChars bits '0'
            let b = this.CountChars bits '1'
            match a with
            | _ when a <= b -> '0'
            | _ -> '1'


        member private this.CountChars (bits: char list) (value: char) =
            bits
            |> Seq.where (fun x -> x = value)
            |> Seq.toList
            |> Seq.length


        member private this.CalcPowerRatingPart (input: string list, func) =
            let count = input[0].Length
            let rec doIt(i: int) =
                match i with
                | _ when i < 0 ->
                    ""
                | _ ->
                    let bits = this.GetdigitsAt(i, input)
                    let x = bits |> func |> string
                    doIt(i - 1) + x
            doIt(count - 1)


        member private this.FindLifeSupportRatingPart (input: string list, func) =
            let rec doIt(i: int, data: string list) = 
                match data with
                | _ when data.Length = 1 ->
                    data[0]
                | _ ->
                    let bits = this.GetdigitsAt(i, data)
                    let x = bits |> func

                    let filteredData = data |> Seq.where (fun item -> item[i] = x) |> Seq.toList
                    doIt(i + 1, filteredData)
            doIt(0, input)


        // What is the power consumption of the submarine?
        member this.RunPart1 (input: string list) =
            let gammaRate = this.CalcPowerRatingPart(input, this.MostCommonBit) |> Helper.BinaryStringToInt
            let epsilonRate = this.CalcPowerRatingPart(input, this.LeastCommonBit) |> Helper.BinaryStringToInt

            gammaRate * epsilonRate


        // What is the life support rating of the submarine?
        member this.RunPart2 (input: string list) =
            let o2GeneratorRating = this.FindLifeSupportRatingPart(input, this.MostCommonBit) |> Helper.BinaryStringToInt
            let co2ScrubberRating = this.FindLifeSupportRatingPart(input, this.LeastCommonBit) |> Helper.BinaryStringToInt

            o2GeneratorRating * co2ScrubberRating



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =
        let input = 
            InputHelper.LoadLines (2021, day, name)
            |> Seq.toList
        
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
        