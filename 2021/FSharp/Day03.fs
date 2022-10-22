namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



type Puzzle03 () =

    member private this.GetdigitsAt(position: int, input: string list) =
        let GetDigitAt(value: string) =
            value[position]
        input |> List.map GetDigitAt


    member private this.MostCommonBit(bits: char list) =
        let a = this.CountChars(bits, '0')
        let b = this.CountChars(bits, '1')
        if a = b then '1' elif a < b then '1' else '0'


    member private this.LeastCommonBit(bits: char list) =
        let a = this.CountChars(bits, '0')
        let b = this.CountChars(bits, '1')
        if a = b then '0' elif a < b then '0' else '1'


    member private this.CountChars(bits: char list, value: char) =
        let result = bits |> Seq.where (fun x -> x = value) |> Seq.toList
        result.Length


    member private this.CalcPowerRatingPart(input: string list, func) =
        let count = input[0].Length
        let rec doIt(i: int) =
            if i < 0 then
                ""
            else
                let bits = this.GetdigitsAt(i, input)
                let x = bits |> func |> string
                doIt(i - 1) + x
        doIt(count - 1)


    member private this.FindLifeSupportRatingPart(input: string list, func) =
        let rec doIt(i: int, data: string list) = 
            if data.Length = 1 then
                data[0]
            else
                let bits = this.GetdigitsAt(i, data)
                let x = bits |> func

                let filteredData = data |> Seq.where (fun item -> item[i] = x) |> Seq.toList
                doIt(i + 1, filteredData)
        doIt(0, input)


    member this.RunPart1 (input: string list) =
        let gammaRate = this.CalcPowerRatingPart(input, this.MostCommonBit) |> Helper.BinaryStringToInt
        let epsilonRate = this.CalcPowerRatingPart(input, this.LeastCommonBit) |> Helper.BinaryStringToInt

        let result = gammaRate * epsilonRate
        result


    member this.RunPart2 (input: string list) =
        let o2GeneratorRating = this.FindLifeSupportRatingPart(input, this.MostCommonBit) |> Helper.BinaryStringToInt
        let co2ScrubberRating = this.FindLifeSupportRatingPart(input, this.LeastCommonBit) |> Helper.BinaryStringToInt

        let result = o2GeneratorRating * co2ScrubberRating
        result
        
        
        
module ``Day 3: Binary Diagnostic`` =
    let private GetPuzzleInput (part:int) (name:string) =
        let day = 3
        
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
        
        let actual = (new Puzzle03()).RunPart1 input
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
        
        
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name
        
        let actual = (new Puzzle03()).RunPart2 input
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
        