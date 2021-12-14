﻿module Day01

open FSharp.Common
open System


type private PuzzleInput(input, expectedAnswer) = 
    inherit InputAnswer<int list, int option>(input, expectedAnswer)


type Day01 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part:int) (name:string) =
        let day = 1

        let input = 
            InputHelper.LoadLines(day, name)
            |> Seq.map int
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt
        
        new PuzzleInput(input, answer)


    member private this.CountIncreases = 
        List.pairwise 
        >> List.filter (fun (a,b) -> a < b) 
        >> List.length


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let result = 
            puzzleData.Input
            |> this.CountIncreases
        Helper.GetPuzzleResultText("How many measurements are larger than the previous measurement?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result =
            puzzleData.Input
            |> List.windowed(3)
            |> List.map List.sum
            |> this.CountIncreases
        Helper.GetPuzzleResultText("How many sums are larger than the previous sum?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 1: Sonar Sweep"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput 1 "example1"))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput 1 "input"))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput 2 "example1"))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput 2 "input"))
       }