module Day01

open FSharp.Common
open Microsoft.FSharp.Linq
open System
open System.Linq


type private Day01PuzzleInput(input, expectedAnswer) = 
    inherit InputAnswer<int list, int option>(input, expectedAnswer)


type Day01 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 1
        let input = 
            InputHelper.LoadLines(day, name)
            |> Seq.map int
            |> Seq.toList
        
        let answer =
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt
        
        new Day01PuzzleInput(input, answer)


    member private this.CalcFuel mass : int =
        (mass / 3) - 2


    member private this.CalcFuelCorrectly mass : int =
        let fuelMass = this.CalcFuel(mass)
        match fuelMass with
        | _ when fuelMass > 0 -> fuelMass + this.CalcFuelCorrectly(fuelMass)
        | _ -> 0
        //if (fuelMass > 0) then
        //    fuelMass + this.CalcFuelCorrectly(fuelMass)
        //else
        //    0


    member private this.RunPart1 (puzzleData: Day01PuzzleInput) =
        let totalFuel = puzzleData.Input |> List.sumBy this.CalcFuel
        Helper.GetPuzzleResultText($"What is the sum of the fuel requirements?", totalFuel, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: Day01PuzzleInput) =
        let totalFuel = puzzleData.Input |> List.sumBy this.CalcFuelCorrectly
        Helper.GetPuzzleResultText($"What is the sum of the fuel requirements?", totalFuel, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 1: The Tyranny of the Rocket Equation"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunExample(fun _ -> " Ex. 2) " + this.RunPart1(this.GetPuzzleInput(1, "example2")))
        yield this.RunExample(fun _ -> " Ex. 3) " + this.RunPart1(this.GetPuzzleInput(1, "example3")))
        yield this.RunExample(fun _ -> " Ex. 4) " + this.RunPart1(this.GetPuzzleInput(1, "example4")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 2) " + this.RunPart2(this.GetPuzzleInput(2, "example2")))
        yield this.RunExample(fun _ -> " Ex. 3) " + this.RunPart2(this.GetPuzzleInput(2, "example3")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }
