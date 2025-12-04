namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 19: Beacon Scanner`` =
    let day = 19



    //-------------------------------------------------------------------------



    type private Puzzle () =

        //How many beacons are there?
        member this.RunPart1 (input:string list) =
            let result = 0
            result


        // 
        //member this.RunPart2 (input:string list) =
        //    let result = 0
        //    result



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
    
    
    //[<Theory>]
    //[<InlineData("example1")>]
    //[<InlineData("input")>]
    //let Part2 (name:string) =
    //    let input, expected = GetPuzzleInput 2 name
    
    //    let actual = (new Puzzle()).RunPart2 input
    
    //    match expected with
    //    | None -> Assert.Null actual
    //    | _ -> Assert.Equal (expected.Value, actual)
    