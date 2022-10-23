// Speed improvements were found by referencing this solution:
// https://github.com/bhosale-ajay/adventofcode/blob/master/2021/fs/D05.fs
namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



type Puzzle05 () =

    member private this.ParseToEndPointStrings (value: string) =
        value.Split(" -> ")
        |> Array.toList

    member private this.parseToIntList (value: string) =
        value.Split(",")
        |> Array.map int
        |> Array.toList

    member private this.ParseStringEndpointToIntList (values: string list) =
        values
        |> List.map this.parseToIntList

    member private this.OnlyFlatLines (z: int list list) =
        z[0][0] = z[1][0] || z[0][1] = z[1][1]

    member private this.MakeRanges (z: int list list) =
        let x1 = z[0][0]
        let y1 = z[0][1]
        let x2 = z[1][0]
        let y2 = z[1][1]
        let xStep = if x1 <= x2 then 1 else -1
        let yStep = if y1 <= y2 then 1 else -1

        let rX = [x1..xStep..x2]
        let rY = [y1..yStep..y2]
        rX, rY

    member private this.MakePoints (value: int list * int list) =
        let a, b = value
        
        match a with
        | _ when a.Length = b.Length ->
            [0..a.Length - 1]
            |> List.map (fun i -> [a[i], b[i]])
            |> List.concat
        | _ ->
            List.allPairs a b


    member private this.ToPointString (value: int * int) =
        let a, b = value
        $"{a},{b}"


    // At how many points do at least two lines overlap?
    member this.RunPart1 (input: string list) =
        input
        |> List.map this.ParseToEndPointStrings
        |> List.map this.ParseStringEndpointToIntList
        |> List.where this.OnlyFlatLines
        |> List.map this.MakeRanges
        |> List.map this.MakePoints
        |> List.concat
        |> List.map this.ToPointString
        |> List.countBy id
        |> List.where (fun (p, c) -> c > 1)
        |> List.length


    // At how many points do at least two lines overlap?
    member this.RunPart2 (input: string list) =
        input
        |> List.map this.ParseToEndPointStrings
        |> List.map this.ParseStringEndpointToIntList
        |> List.map this.MakeRanges
        |> List.map this.MakePoints
        |> List.concat
        |> List.map this.ToPointString
        |> List.countBy id
        |> List.where (fun (p, c) -> c > 1)
        |> List.length



module ``Day 05: Hydrothermal Venture`` =
    let private GetPuzzleInput (part:int) (name:string) =
        let day = 5
        
        let input =
            InputHelper.LoadLines(day, name)
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
        
        let actual = (new Puzzle05()).RunPart1 input
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
        
        
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name
        
        let actual = (new Puzzle05()).RunPart2 input
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
