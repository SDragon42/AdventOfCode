namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 18: Snailfish`` =
    let day = 18



    //-------------------------------------------------------------------------



    type private SFNumber (left, right) =
        member this.Left:string = left
        member this.Right:string = right



    //-------------------------------------------------------------------------



    type Puzzle () =

        member private this.ParseNumber (number:char list) =
            let a = number.Head
            match a with
            | '[' ->
                    let left = ""
                    let right = ""
                    SFNumber (left, right)
            | _ ->
                    SFNumber ("", "")


        member this.Reduce (number:string) =
            ""


        //member private this.TestReduce (number:string) (expected:string) =
        //    let result = this.Reduce number
        //    Helper.GetPuzzleResultText ("    Reduce:", result, expected |> Some)


        //member this.RunTests () = 
        //    let results = seq {
        //        yield this.TestReduce "[[[[[9,8],1],2],3],4]" "[[[[0,9],2],3],4]"
        //        yield this.TestReduce "[7,[6,[5,[4,[3,2]]]]]" "[7,[6,[5,[7,0]]]]"
        //        yield this.TestReduce "[[6,[5,[4,[3,2]]]],1]" "[[6,[5,[7,0]]],3]"
        //        yield this.TestReduce "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]" "[[3,[2,[8,0]]],[9,[5,[7,0]]]]"
        //        }
        //    let aa = String.Join("\r\n", results)
        //    "\r\n" + aa


        // What is the magnitude of the final sum?
        //member this.RunPart1 (puzzleData: PuzzleInput) =
        //    let result = 0
        //    Helper.GetPuzzleResultText ("", result, puzzleData.ExpectedAnswer)


        // 
        //member this.RunPart2 (puzzleData: PuzzleInput) =
        //    let result = 0
        //    Helper.GetPuzzleResultText ("", result, puzzleData.ExpectedAnswer)
            
            
            
    //-------------------------------------------------------------------------
            


    let private GetPuzzleInput (part:int) (name:string) =
        let input = 
            InputHelper.LoadLines (day, name)
            |> Seq.toList
                    
        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt
                        
        input, answer
                    

    [<Theory>]
    [<InlineData("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")>]
    [<InlineData("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")>]
    [<InlineData("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")>]
    [<InlineData("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")>]
    let TestReducer (input:string) (expected:string) =
        let actual = (new Puzzle()).Reduce input
                    
        Assert.Equal (expected, actual)

                    
    //[<Theory>]
    //[<InlineData("example1")>]
    //[<InlineData("input")>]
    //let Part1 (name:string) =
    //    let input, expected = GetPuzzleInput 1 name
                    
    //    let actual = (new Puzzle()).RunPart1 input
                    
    //    match expected with
    //    | None -> Assert.Null actual
    //    | _ -> Assert.Equal (expected.Value, actual)
                    
                    
    //[<Theory>]
    //[<InlineData("example1")>]
    //[<InlineData("input")>]
    //let Part2 (name:string) =
    //    let input, expected = GetPuzzleInput 2 name
                    
    //    let actual = (new Puzzle()).RunPart2 input
                    
    //    match expected with
    //    | None -> Assert.Null actual
    //    | _ -> Assert.Equal (expected.Value, actual)
