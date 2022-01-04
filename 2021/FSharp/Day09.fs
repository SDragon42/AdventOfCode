module Day09

open FSharp.Common
open System


type private PuzzleInput(input, xMax: int, yMax: int, expectedAnswer) =
    inherit InputAnswer<int list, int option>(input, expectedAnswer)
    member this.xMax = xMax
    member this.yMax = yMax


type Day09 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 9

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

        new PuzzleInput(input, xMax, yMax, answer)


    member private this.offsets = [
        ( 0,-1);
        ( 1, 0);
        ( 0, 1);
        (-1, 0);
        ]

    
    member private this.CoordinatesToIndex(x: int, y: int, puzzleData: PuzzleInput) =
        if x < 0 || x >= puzzleData.xMax then
            None
        elif y < 0 || y >= puzzleData.yMax then
            None
        else
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
                |> List.map (fun i -> puzzleData.Input[i])
                |> List.forall (fun v -> v > value)
            if isLowPoint
            then (idx, value) |> Some
            else None

        puzzleData.Input
            |> List.mapi IsLowestPoint
            |> List.where (fun v -> v.IsSome)
            |> List.map (fun z -> z.Value)


    member private this.GetBasinSize(idx: int, puzzleData: PuzzleInput) =
        let mutable basinIndexes: int list = [idx]
        let rec DoIt(idx: int) =
            let areaSize = 1
            let adjacentIndexes = 
                this.GetAdjacent(idx, puzzleData)
                |> List.map (fun i -> i, puzzleData.Input[i])
                |> List.where (fun (i, v) -> v < 9)
                |> List.map (fun (i, v) -> i)
                |> List.except basinIndexes
            
            if adjacentIndexes.Length = 0 then
                areaSize
            else
                basinIndexes <- basinIndexes @ adjacentIndexes
                let subSize =
                    adjacentIndexes
                    |> List.map (fun i -> DoIt(i))
                    |> List.sum
                areaSize + subSize

        let result = DoIt(idx)
        result


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let lowPoints = this.GetLowPoints(puzzleData)
        let result =
            lowPoints
            |> List.map (fun (_, v) -> v + 1)
            |> List.sum
        Helper.GetPuzzleResultText("What is the sum of the risk levels of all low points on your heightmap?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
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
        Helper.GetPuzzleResultText("What do you get if you multiply together the sizes of the three largest basins?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 9: Smoke Basin"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }