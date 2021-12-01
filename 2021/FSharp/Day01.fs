module Day01

open FSharp.Common
open Microsoft.FSharp.Linq
open System
open System.Linq


type private PuzzleInput(input: int[], expectedAnswer: int option) = 
    inherit InputAnswer<int [], int option>(input, expectedAnswer)


type Day01 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 1

        let input = InputHelper.LoadInputFile(day, name).Split(Environment.NewLine) |> Seq.map int |> Seq.toArray
        
        let GetAnswer(name: string) =
            let text = InputHelper.LoadInputFile(day, $"%s{name}-answer%i{part}")
            try
                text |> int |> Some
            with
                | ex -> None
        let answer = GetAnswer(name)
        
        new PuzzleInput(input, answer)


    member private this.CheckMeasurement (windowSize: int, last: int, remaining: int[]) =
        if remaining.Length < windowSize then
            0
        else
            let next = remaining[..windowSize - 1] |> Array.sum
            let value = if next > last && last > 0 then 1 else 0
            value + this.CheckMeasurement(windowSize, next, remaining[1..])


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let result = this.CheckMeasurement(1, 0, puzzleData.Input)
        Helper.GetPuzzleResultText("How many measurements are larger than the previous measurement?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result = this.CheckMeasurement(3, 0, puzzleData.Input)
        Helper.GetPuzzleResultText("How many sums are larger than the previous sum?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 1: Sonar Sweep"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }