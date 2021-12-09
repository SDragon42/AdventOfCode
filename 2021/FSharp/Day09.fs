module Day09

open FSharp.Common
open System


type private PuzzleInput(input, xMax: int, expectedAnswer) =
    inherit InputAnswer<int list, int option>(input, expectedAnswer)
    member this.xMax = xMax


type Day09 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 9

        let rawinput =
            InputHelper.LoadLines(day, name)
            |> Seq.toList
        let xMax = rawinput[0].Length

        let input =
            rawinput
            |> Seq.map (fun l -> l |> Seq.toList |> List.map string |> List.map int)
            |> Seq.concat
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput(input, xMax, answer)


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let yMax = puzzleData.Input.Length / puzzleData.xMax
        let offsets = [(0, -1);(1,0);(0,1);(-1,0)]

        let GetValue(x: int, y: int) =
            if x < 0 || x >= puzzleData.xMax then
                None
            elif y < 0 || y >= yMax then
                None
            else
                let idx = (y * puzzleData.xMax) + x
                let value = puzzleData.Input[idx] |> Some
                value

        let IsLowestPoint idx value =
            let y = idx / puzzleData.xMax
            let x = idx - (y * puzzleData.xMax)

            let adjacent1 = offsets |> List.map (fun (ox, oy) -> GetValue(x+ox, y+oy))
            let adjacent2 = adjacent1 |> List.where (fun i -> i.IsSome)
            let adjacent3 = adjacent2 |> List.map (fun i -> i.Value)
            let isLowPoint = adjacent3 |> List.forall (fun i -> i > value)
            if isLowPoint
            then value |> Some
            else None

        let lowPoints =
            puzzleData.Input
            |> List.mapi IsLowestPoint
            |> List.where (fun v -> v.IsSome)
            |> List.map (fun v -> v.Value)

        let result =
            lowPoints
            |> List.map (fun v -> v + 1)
            |> List.sum
        Helper.GetPuzzleResultText("What is the sum of the risk levels of all low points on your heightmap?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText("", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 9: Smoke Basin"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        //yield ""
        //yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        //yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }