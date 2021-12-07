module Day07

open FSharp.Common
open System


type private PuzzleInput(input, expectedAnswer) =
    inherit InputAnswer<int list, int option>(input, expectedAnswer)


type Day07 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 7

        let input =
            InputHelper.LoadText(day, name).Split(',')
            |> Array.toList
            |> List.map int

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput(input, answer)


    member private this.CalcMedian (data: int list) =
        let CalcMedianOdd (data: int list) =
            let p = data.Length + 1 / 2
            data[p - 1]
        let CalcMedianEven (data: int list) =
            let p = data.Length / 2
            (data[p - 1] + data[p]) / 2

        let orderedData = data |> List.sortBy (fun t -> t)

        if (data.Length % 2) = 0
            then CalcMedianEven(orderedData)
            else CalcMedianOdd(orderedData)

    
    member private this.CalcMeans (data: int list) =
        let sum = data |> List.sum
        let mean = sum / data.Length
        [mean - 1; mean; mean + 1]

    
    member private this.CalcFuel1(position: int, target: int) =
        let fuel = abs(position - target)
        fuel


    member private this.CalcFuel2(position: int, target: int) =
        let distance = abs(position - target)
        let fuel = (distance * (distance + 1)) / 2
        fuel


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let median = this.CalcMedian(puzzleData.Input)

        let result =
            puzzleData.Input
            |> List.map (fun h -> this.CalcFuel1(h, median))
            |> List.sum

        Helper.GetPuzzleResultText("How much fuel must they spend to align to that position?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let DoIt (target: int) = 
            puzzleData.Input 
            |> List.map (fun h -> this.CalcFuel2(h, target))
            |> List.sum

        let result = 
            this.CalcMeans(puzzleData.Input)
            |> List.map DoIt 
            |> List.min

        Helper.GetPuzzleResultText("How much fuel must they spend to align to that position?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 7: The Treachery of Whales"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }