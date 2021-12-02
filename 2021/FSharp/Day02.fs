module Day02

open FSharp.Common
open Microsoft.FSharp.Linq
open System
open System.Linq



type private PilotAction =
    val Direction: string
    val Amount: int

    new(direction, amount) = { Direction = direction; Amount = amount}
    


type private PuzzleInput(input: PilotAction[], expectedAnswer: int option) = 
    inherit InputAnswer<PilotAction [], int option>(input, expectedAnswer)



type Day02 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)

    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 2

        let makePilotAction (value: string) =
            let parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            new PilotAction(parts[0], parts[1] |> int)

        let input = InputHelper.LoadInputFile(day, name).Split(Environment.NewLine) |> Seq.map makePilotAction |> Seq.toArray
        
        let GetAnswer(name: string) =
            let text = InputHelper.LoadInputFile(day, $"%s{name}-answer%i{part}")
            try
                text |> int |> Some
            with
                | ex -> None
        let answer = GetAnswer(name)
        
        new PuzzleInput(input, answer)


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let mutable horizontal = 0
        let mutable depth = 0

        for a in puzzleData.Input do
            match a.Direction with
            | "forward" -> horizontal <- horizontal + a.Amount
            | "down" -> depth <- depth + a.Amount
            | "up" -> depth <- depth - a.Amount
            | _ -> failwith $"invalid input key {a.Direction} {a.Amount}"
        
        let result = horizontal * depth

        Helper.GetPuzzleResultText("What do you get if you multiply your final horizontal position by your final depth?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let mutable horizontal = 0
        let mutable depth = 0
        let mutable aim = 0

        for a in puzzleData.Input do
            match a.Direction with
            | "forward" -> 
                    horizontal <- horizontal + a.Amount
                    depth <- depth + (aim * a.Amount)
            | "down" -> aim <- aim + a.Amount
            | "up" -> aim <- aim - a.Amount
            | _ -> failwith $"invalid input key {a.Direction} {a.Amount}"
        
        let result = horizontal * depth

        Helper.GetPuzzleResultText("What do you get if you multiply your final horizontal position by your final depth?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 2: Dive!"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }