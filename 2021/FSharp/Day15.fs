// Understanding Dijkstra's Algorithm
// http://blog.aos.sh/2018/02/24/understanding-dijkstras-algorithm/
namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 15: Chiton`` =
    let day = 15



    //-------------------------------------------------------------------------



    type private PuzzleInput (input: int list, xMax: int, yMax: int) =
        member this.input = input
        member this.xMax = xMax
        member this.yMax = yMax



    //-------------------------------------------------------------------------
    
    
    
    type private Puzzle () =

        [<DefaultValue>]
        val mutable grid: int list
        [<DefaultValue>]
        val mutable xMax: int
        [<DefaultValue>]
        val mutable yMax: int


        member private this.SetClassVars (puzzleData: PuzzleInput) =
            this.grid <- puzzleData.input
            this.xMax <- puzzleData.xMax
            this.yMax <- puzzleData.yMax


        member private this.Offsets2 = [
            ( 1, 0);
            ( 0, 1);
            ]


        member private this.FindAllPaths (position:int) (path:int list) =
            //let BuildPath (idx:int) =
            //    //let newPath = path @ [idx]
            //    this.FindAllPaths idx path

            match position with
            | _ when position = this.grid.Length - 1 ->
                [path @ [position]]
            | _ ->
                let newPath = path @ [position]
                let found =
                    GridHelper.GetAdjacentIndexes (position, this.xMax, this.grid.Length, this.Offsets2)
                    |> List.except path
                    |> List.map (fun i -> this.FindAllPaths i newPath)
                    |> List.concat
                found


        // What is the lowest total risk of any path from the top left to the bottom right?
        member this.RunPart1 (puzzleData: PuzzleInput) =
            this.SetClassVars puzzleData
            let origin = 0
            let allPaths = this.FindAllPaths origin []

            let paths2 =
                allPaths
                |> List.map (fun l -> (l |> List.map (fun z -> this.grid[z]) |> List.sum), l)
                |> List.sortBy (fun (risk, _) -> risk)

            let totalRisk, _ = paths2 |> List.take 1 |> List.exactlyOne

            let result = totalRisk - this.grid[origin]
            result


        //member this.RunPart2 (puzzleData: PuzzleInput) =
        //    let result = 0
        //    Helper.GetPuzzleResultText ("", result, puzzleData.ExpectedAnswer)

        
        
    //-------------------------------------------------------------------------
        


    let private GetPuzzleInput (part:int) (name:string) =
        let rawinput =
            InputHelper.LoadLines (day, name)
            |> Seq.toList
        let xMax = rawinput[0].Length
        let yMax = rawinput.Length
        
        let input =
            rawinput
            |> List.map (fun s -> s |> Seq.toList |> List.map (fun c -> c |> string |> int) )
            |> List.concat
        
        
        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt
                
        let data = new PuzzleInput(input, xMax, yMax)
        data, answer
            
            
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let puzzleData, expected = GetPuzzleInput 1 name
            
        let actual = (new Puzzle()).RunPart1 puzzleData
            
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
            
            
    //[<Theory>]
    //[<InlineData("example1")>]
    //[<InlineData("input")>]
    //let Part2 (name:string) =
    //    let puzzleData, expected = GetPuzzleInput 2 name
            
    //    let actual = (new Puzzle()).RunPart2 puzzleData
            
    //    match expected with
    //    | None -> Assert.Null actual
    //    | _ -> Assert.Equal (expected.Value, actual)
            