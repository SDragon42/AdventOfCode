module Day18

open FSharp.Common
open System


type private SFNumber (left, right) =
    member this.Left:string = left
    member this.Right:string = right


type private PuzzleInput (input, expectedAnswer) =
    inherit InputAnswer<string list, int option> (input, expectedAnswer)


type Day18 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part:int) (name: string) =
        let day = 18

        let input =
            InputHelper.LoadLines (day, name)
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput (input, answer)


    member private this.ParseNumber (number:char list) =
        let a = number.Head
        match a with
        | '[' ->
                let left = ""
                let right = ""
                SFNumber (left, right)
        | _ ->
                SFNumber ("", "")
    member private this.Reduce (number:string) =
        ""


    member private this.TestReduce (number:string) (expected:string) =
        let result = this.Reduce number
        Helper.GetPuzzleResultText ("    Reduce:", result, expected |> Some)


    member private this.RunTests () = 
        let results = seq {
            yield this.TestReduce "[[[[[9,8],1],2],3],4]" "[[[[0,9],2],3],4]"
            yield this.TestReduce "[7,[6,[5,[4,[3,2]]]]]" "[7,[6,[5,[7,0]]]]"
            yield this.TestReduce "[[6,[5,[4,[3,2]]]],1]" "[[6,[5,[7,0]]],3]"
            yield this.TestReduce "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]" "[[3,[2,[8,0]]],[9,[5,[7,0]]]]"
            }
        let aa = String.Join("\r\n", results)
        "\r\n" + aa



    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText ("What is the magnitude of the final sum?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText ("", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 18: Snailfish"
        yield this.RunExample (fun _ -> "TESTS ) " + this.RunTests () )
        // yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput 1 "example1") )
        // yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput 1 "input") )

        // yield ""
        // yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput 2 "example1") )
        // yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput 2 "input") )
        }