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


    member private this.CalcFuel mass : int =
        (mass / 3) - 2


    member private this.CalcFuelCorrectly mass : int =
        let fuelMass = (mass / 3) - 2
        match fuelMass with
        | _ when fuelMass > 0 -> fuelMass + this.CalcFuelCorrectly(fuelMass)
        | _ -> 0
        //if (fuelMass > 0) then
        //    fuelMass + this.CalcFuelCorrectly(fuelMass)
        //else
        //    0


    member private this.ShowResult (value: int, expected: int option) =
        Helper.GetPuzzleResultText($"Total Fuel needed {value}", value, expected)


    member private this.RunPart1 (puzzleData: Day01PuzzleInput) =
        let totalFuel = puzzleData.Input |> Array.sumBy this.CalcFuel
        this.ShowResult(totalFuel, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: Day01PuzzleInput) =
        let totalFuel = puzzleData.Input |> Array.sumBy this.CalcFuelCorrectly
        this.ShowResult(totalFuel, puzzleData.ExpectedAnswer)


    member private this.Example1 : string = " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1"))
    member private this.Example2 : string = " Ex. 2) " + this.RunPart1(this.GetPuzzleInput(1, "example2"))
    member private this.Example3 : string = " Ex. 3) " + this.RunPart1(this.GetPuzzleInput(1, "example3"))
    member private this.Example4 : string = " Ex. 4) " + this.RunPart1(this.GetPuzzleInput(1, "example4"))
    member private this.Part1 : string = "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input"))

    member private this.Example2P2 : string = " Ex. 2) " + this.RunPart2(this.GetPuzzleInput(2, "example2"))
    member private this.Example3P2 : string = " Ex. 3) " + this.RunPart2(this.GetPuzzleInput(2, "example3"))
    member private this.Part2 : string = "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input"))

    override this.SolvePuzzle _ = seq {
        yield "Day 1: The Tyranny of the Rocket Equation"
        yield this.RunExample(fun _ -> this.Example1)
        yield this.RunExample(fun _ -> this.Example2)
        yield this.RunExample(fun _ -> this.Example3)
        yield this.RunExample(fun _ -> this.Example4)
        yield this.Run(fun _ -> this.Part1)

        yield ""
        yield this.RunExample(fun _ -> this.Example2P2)
        yield this.RunExample(fun _ -> this.Example3P2)
        yield this.Run(fun _ -> this.Part2)
        }
