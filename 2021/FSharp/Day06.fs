namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 06: Lanternfish`` =
    let day = 6



    //-------------------------------------------------------------------------



    type private Puzzle () =

        member private this.FishSpawnTimer: int = 6
        member private this.NewFishSpawnTimer: int = 8


        member private this.SumInitialFishGroups (fishGroup: int * int list) =
            let group, fishList = fishGroup
            group, fishList.Length |> uint64


        member private this.DecrementFishSpawningTimer (fishGroup: int * uint64) =
            let group, count = fishGroup
            match group with
            | _ when group > 0 ->
                group - 1, count
            | _ ->
                this.FishSpawnTimer, count


        member private this.IsSpawningFishGroup (fishGroup: int * uint64) =
            let group, _ = fishGroup
            group = 0


        member private this.NewSpawningFishGroup (fishGroup: int * uint64) =
            let _, number = fishGroup
            this.NewFishSpawnTimer, number


        member private this.GetFishGroup (fishGroup: int * uint64) =
            let group, _ = fishGroup
            group


        member private this.GetFishCount (fishGroup: int * uint64) =
            let _, count = fishGroup
            count


        member private this.SumFishGroups (fishGroup: int * (int * uint64) list) =
            let group, oldGroupList = fishGroup
            let count = 
                oldGroupList 
                |> List.map this.GetFishCount
                |> List.sum
            group, count


        member private this.RunSimulation(input: int list, maxNumDays: int) =
            let newPuzzleData =
                input
                |> List.groupBy (fun f -> f)
                |> List.map this.SumInitialFishGroups

            let rec DoIt (day: int) (lanternFish: (int * uint64) list) =
                match day with
                | _ when day = maxNumDays ->
                    lanternFish
                | _ ->
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

                    DoIt (day + 1) newLanternFish
        
            let totalLanternFish = 
                DoIt 0 newPuzzleData
                |> List.map this.GetFishCount
                |> List.sum
            totalLanternFish


        // How many lanternfish would there be after 80 days?
        member this.RunPart1 (input: int list) =
            this.RunSimulation(input, 80)


        // How many lanternfish would there be after 256 days?
        member this.RunPart2 (input: int list) =
            this.RunSimulation(input, 256)



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =
        let input =
            InputHelper.LoadText(day, name).Split(',')
            |> Seq.map int
            |> Seq.toList
        
        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsUInt64
            
        input, answer
        
        
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let input, expected = GetPuzzleInput 1 name
        
        let actual = (new Puzzle()).RunPart1 input
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
        
        
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name
        
        let actual = (new Puzzle()).RunPart2 input
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
