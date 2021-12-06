module Day06

open FSharp.Common
open Microsoft.FSharp.Linq
open System
open System.Linq


type private PuzzleInput(input, expectedAnswer) =
    inherit InputAnswer<int list, uint64 option>(input, expectedAnswer)


type Day06 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 6

        let input = InputHelper.LoadInputFile(day, name).Split(',') |> Array.toList |> List.map int

        let GetAnswer(name: string) =
            let text = InputHelper.LoadInputFile(day, $"%s{name}-answer%i{part}")
            try
                text |> uint64 |> Some
            with
                | ex -> None
        let answer = GetAnswer(name)

        new PuzzleInput(input, answer)


    member private this.FishSpawnTimer: int = 6
    member private this.NewFishSpawnTimer: int = 8


    member private this.SumInitialFishGroups (fishGroup: int * int list) =
        let fish, fishList = fishGroup
        fish, fishList.Length |> uint64

    member private this.DecrementFishSpawningTimer (fishGroup: int * uint64) =
        let group, number = fishGroup
        if (group > 0)
            then group - 1, number
            else this.FishSpawnTimer, number

    member private this.IsSpawningFishGroup (fishGroup: int * uint64) =
        let group, _ = fishGroup
        group = 0

    member private this.NewSpawningFishGroup (fishGroup: int * uint64) =
        let _, number = fishGroup
        this.NewFishSpawnTimer, number

    member private this.GetFishGroup (fishGroup: int * uint64) =
        let group, _ = fishGroup
        group

    member private this.SumFishGroups (fishGroup: int * (int * uint64) list) =
        let getFishNum (f: int * uint64) =
            let _, numFish = f
            numFish

        let group, oldGroupList = fishGroup
        let count = 
            oldGroupList 
            |> List.map getFishNum
            |> List.sum
        group, count


    member private this.RunSimulation(input: int list, maxNumDays: int) =
        let newPuzzleData =
            input
            |> List.groupBy (fun f -> f)
            |> List.map this.SumInitialFishGroups

        let rec DoIt (day: int, lanternFish: (int * uint64) list) =
            if (day = maxNumDays) then
                lanternFish
            else
                let fishListA = 
                    lanternFish 
                    |> List.map this.DecrementFishSpawningTimer
                let fishListB =
                    lanternFish 
                    |> List.where this.IsSpawningFishGroup
                    |> List.map this.NewSpawningFishGroup

                let newLanternFish = 
                    fishListA 
                    |> List.append fishListB
                    |> List.groupBy this.GetFishGroup
                    |> List.map this.SumFishGroups

                DoIt (day + 1, newLanternFish)
        
        let lanternFish = DoIt(0, newPuzzleData)

        let GetTotals (fishGroup: int * uint64) =
            let _, number = fishGroup
            number
            
        lanternFish |> List.map GetTotals |> List.sum


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let result = this.RunSimulation(puzzleData.Input, 80)
        Helper.GetPuzzleResultText("How many lanternfish would there be after 80 days?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result = this.RunSimulation(puzzleData.Input, 256)
        Helper.GetPuzzleResultText("How many lanternfish would there be after 256 days?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 6: Lanternfish"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }