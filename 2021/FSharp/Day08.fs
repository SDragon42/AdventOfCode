module Day08

open FSharp.Common
open System


type private PuzzleInput(input, expectedAnswer) =
    inherit InputAnswer<(string list * string list) list, int option>(input, expectedAnswer)



type Day08 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)

    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 8

        let SplitData (line: string) =
            let parts = line.Split('|', 2)
            let signal = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map (fun i -> i.Trim()) |> Array.toList
            let digits = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map (fun i -> i.Trim()) |> Array.toList
            signal, digits


        let input =
            InputHelper.LoadLines(day, name)
            |> Seq.map SplitData
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput(input, answer)


    member this.IsDigit_1(pattern: string) =
        pattern.Length = 2

    member this.IsDigit_4(pattern: string) =
        pattern.Length = 4

    member this.IsDigit_7(pattern: string) =
        pattern.Length = 3

    member this.IsDigit_8(pattern: string) =
        pattern.Length = 7


    member this.ProcessDigits (data: (string list * string list)) = 
        let _, digits = data
        let count =
            (digits |> List.where this.IsDigit_1 |> List.length) +
            (digits |> List.where this.IsDigit_4 |> List.length) +
            (digits |> List.where this.IsDigit_7 |> List.length) + 
            (digits |> List.where this.IsDigit_8 |> List.length)
        count


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let result = 
            puzzleData.Input
            |> List.map this.ProcessDigits
            |> List.sum

        Helper.GetPuzzleResultText("In the output values, how many times do digits 1, 4, 7, or 8 appear?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result = 
            puzzleData.Input
            |> List.map this.ProcessDigits
            |> List.sum

        Helper.GetPuzzleResultText("What do you get if you add up all of the output values?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 8: Seven Segment Search"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        //yield ""
        //yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        //yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }