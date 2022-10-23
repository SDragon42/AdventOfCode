namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit


module ``Day 09: Smoke Basin`` =
    let day = 9



    //-------------------------------------------------------------------------



    type private PuzzleInput(input: int list, xMax: int, yMax: int) =
        member this.input = input
        member this.xMax = xMax
        member this.yMax = yMax



    //-------------------------------------------------------------------------
    
    
    
    type private Puzzle () =

        member private this.offsets = [
            ( 0,-1);
            ( 1, 0);
            ( 0, 1);
            (-1, 0);
            ]

    
        member private this.CoordinatesToIndex(x: int, y: int, puzzleData: PuzzleInput) =
            match None with
            | _ when x < 0 || x >= puzzleData.xMax -> None
            | _ when y < 0 || y >= puzzleData.yMax -> None
            | _ ->
                let idx = (y * puzzleData.xMax) + x
                idx |> Some


        member private this.GetAdjacent(idx: int, puzzleData: PuzzleInput) =
            let y = idx / puzzleData.xMax
            let x = idx - (y * puzzleData.xMax)

            this.offsets 
            |> List.map (fun (ox, oy) -> this.CoordinatesToIndex(x+ox, y+oy, puzzleData))
            |> List.where (fun i -> i.IsSome)
            |> List.map (fun i -> i.Value)


        member private this.GetLowPoints(puzzleData: PuzzleInput) =
            let IsLowestPoint idx value =
                let isLowPoint =
                    this.GetAdjacent(idx, puzzleData)
                    |> List.map (fun i -> puzzleData.input[i])
                    |> List.forall (fun v -> v > value)

                match isLowPoint with
                | true -> (idx, value) |> Some
                | _ -> None

            puzzleData.input
                |> List.mapi IsLowestPoint
                |> List.where (fun v -> v.IsSome)
                |> List.map (fun z -> z.Value)


        member private this.GetBasinSize(idx: int, puzzleData: PuzzleInput) =
            let mutable basinIndexes: int list = [idx]
            let rec DoIt(idx: int) =
                let areaSize = 1
                let adjacentIndexes = 
                    this.GetAdjacent(idx, puzzleData)
                    |> List.map (fun i -> i, puzzleData.input[i])
                    |> List.where (fun (i, v) -> v < 9)
                    |> List.map (fun (i, v) -> i)
                    |> List.except basinIndexes
            
                match adjacentIndexes.Length with
                | 0 ->
                    areaSize
                | _ ->
                    basinIndexes <- basinIndexes @ adjacentIndexes
                    let subSize =
                        adjacentIndexes
                        |> List.map (fun i -> DoIt(i))
                        |> List.sum
                    areaSize + subSize

            let result = DoIt(idx)
            result


        // What is the sum of the risk levels of all low points on your heightmap?
        member this.RunPart1 (puzzleData: PuzzleInput) =
            let lowPoints = this.GetLowPoints(puzzleData)
            let result =
                lowPoints
                |> List.map (fun (_, v) -> v + 1)
                |> List.sum
            result


        // What do you get if you multiply together the sizes of the three largest basins?
        member this.RunPart2 (puzzleData: PuzzleInput) =
            let lowPoints = this.GetLowPoints(puzzleData)

            let basins1 =
                lowPoints
                |> List.map (fun (i, _) -> this.GetBasinSize(i, puzzleData))

            let basins =
                basins1
                |> List.sortDescending
                |> List.take(3)
                |> Seq.toList

            let result = List.fold (fun a b -> a * b) 1 basins
            result



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =
        let rawinput =
            InputHelper.LoadLines(day, name)
            |> Seq.toList
        let xMax = rawinput[0].Length
        let yMax = rawinput.Length

        let input =
            rawinput
            |> Seq.map (fun l -> l |> Seq.toList |> List.map string |> List.map int)
            |> Seq.concat
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
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
                
                
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let puzzleData, expected = GetPuzzleInput 2 name
                
        let actual = (new Puzzle()).RunPart2 puzzleData
                
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
