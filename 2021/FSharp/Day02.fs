module Day02

open FSharp.Common
open System


type private PilotAction =
    val Direction: string
    val Amount: int

    new(direction, amount) = { Direction = direction; Amount = amount}



type private PuzzleInput(input, expectedAnswer) =
    inherit InputAnswer<PilotAction list, int option>(input, expectedAnswer)



type Day02 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)

    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 2

        let makePilotAction (value: string) =
            let parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            new PilotAction(parts[0], parts[1] |> int)

        let input = 
            InputHelper.LoadLines(day, name)
            |> Seq.map makePilotAction
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput(input, answer)


    member private this.CalculateSubPosition_Flawed(input: PilotAction list, (position, depth): int * int) =
        if (input.Length = 0) then
            (position, depth)
        else
            let next = input[0]
            let remaining = input[1..]

            let values =
                match next.Direction with
                | "forward" -> (
                    position + next.Amount,
                    depth)
                | "down" -> (
                    position,
                    depth + next.Amount)
                | "up" -> (
                    position,
                    depth - next.Amount)
                | _ -> (position, depth)

            this.CalculateSubPosition_Flawed(remaining, values)


    member private this.CalculateSubPosition(input: PilotAction list, (position, depth, aim): int * int * int) =
        if (input.Length = 0) then
            (position, depth, aim)
        else
            let next = input[0]
            let remaining = input[1..]

            let values =
                match next.Direction with
                | "forward" -> (
                    position + next.Amount,
                    depth + (aim * next.Amount),
                    aim)
                | "down" -> (
                    position,
                    depth,
                    aim + next.Amount)
                | "up" -> (
                    position,
                    depth,
                    aim - next.Amount)
                | _ -> (position, depth, aim)

            this.CalculateSubPosition(remaining, values)


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let horizontal, depth = this.CalculateSubPosition_Flawed(puzzleData.Input, (0, 0))

        let result = horizontal * depth

        Helper.GetPuzzleResultText("What do you get if you multiply your final horizontal position by your final depth?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let horizontal, depth, _ = this.CalculateSubPosition(puzzleData.Input, (0, 0, 0))

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