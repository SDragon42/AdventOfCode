module Day15

open FSharp.Common
open System
// Understanding Dijkstra's Algorithm
// http://blog.aos.sh/2018/02/24/understanding-dijkstras-algorithm/

type private PuzzleInput (input, xMax: int, yMax: int, expectedAnswer) =
    inherit InputAnswer<int list, int option> (input, expectedAnswer)
    member this.xMax = xMax
    member this.yMax = yMax


type Day15 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)

    [<DefaultValue>]
    val mutable grid: int list
    [<DefaultValue>]
    val mutable xMax: int
    [<DefaultValue>]
    val mutable yMax: int


    member private this.GetPuzzleInput (part:int) (name: string) =
        let day = 15

        let rawinput = InputHelper.LoadLines (day, name) |> Seq.toList
        let xMax = rawinput[0].Length
        let yMax = rawinput.Length

        let input =
            rawinput
            |> List.map (fun s -> s |> Seq.toList |> List.map (fun c -> c |> string |> int) )
            |> List.concat

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput (input, xMax, yMax, answer)


    member private this.SetClassVars (puzzleData: PuzzleInput) =
        this.grid <- puzzleData.Input
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


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        this.SetClassVars puzzleData
        let origin = 0
        let allPaths = this.FindAllPaths origin []

        let paths2 =
            allPaths
            |> List.map (fun l -> (l |> List.map (fun z -> this.grid[z]) |> List.sum), l)
            |> List.sortBy (fun (risk, _) -> risk)

        let totalRisk, _ = paths2 |> List.take 1 |> List.exactlyOne

        let result = totalRisk - this.grid[origin]
        Helper.GetPuzzleResultText ("What is the lowest total risk of any path from the top left to the bottom right?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText ("", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 15: Chiton"
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput 1 "example1") )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput 1 "input") )

        //yield ""
        //yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput 2 "example1") )
        //yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput 2 "input") )
        }