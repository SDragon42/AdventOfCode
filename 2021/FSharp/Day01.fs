module Day01

open FSharp.Common
open Microsoft.FSharp.Linq
open System
open System.Linq


type private Day01PuzzleInput(input: int[], expectedAnswer: int option) = 
    inherit InputAnswer<int [], int option>(input, expectedAnswer)


type Day01 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)

    let DAY = 1


    member private this.GetPuzzleInput (part: int, name: string) =
        let input = InputHelper.LoadInputFile(DAY, name).Split(Environment.NewLine) |> Seq.map int |> Seq.toArray
        
        let GetAnswer(name: string) =
            let text = InputHelper.LoadInputFile(DAY, $"%s{name}-answer%i{part}")
            try
                text |> int |> Some
            with
                | ex -> None
        let answer = GetAnswer(name)
        
        new Day01PuzzleInput(input, answer)



    member private this.RunPart1 (puzzleData: Day01PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText("", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: Day01PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText("", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 1: "
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 2) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }