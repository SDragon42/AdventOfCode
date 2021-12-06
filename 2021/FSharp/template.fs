module Template

open FSharp.Common
open Microsoft.FSharp.Linq
open System
open System.Linq


type private PuzzleInput(input, expectedAnswer) =
    inherit InputAnswer<string list, int option>(input, expectedAnswer)


type Template (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 0

        let input = InputHelper.LoadInputFile(day, name).Split(Environment.NewLine) |> Array.toList

        let GetAnswer(name: string) =
            let text = InputHelper.LoadInputFile(day, $"%s{name}-answer%i{part}")
            try text |> int |> Some
            with | ex -> None
        let answer = GetAnswer(name)

        new PuzzleInput(input, answer)


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText("", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText("", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "TITLE"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }