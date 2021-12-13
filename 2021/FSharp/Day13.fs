module Day13

open FSharp.Common
open System


type private Fold (kind, value) =
    member this.Kind = kind
    member this.Value = value


type private PuzzleInput (input, folds, expectedAnswer) =
    inherit InputAnswer<(int * int) list, string option> (input, expectedAnswer)
    member this.Folds = folds


type Day13 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part:int) (name:string) =
        let day = 13

        let inputParts = InputHelper.LoadText(day, name).Split("\r\n\r\n")

        let input =
            inputParts[0].Split("\r\n")
            |> Seq.map (fun l -> l.Split(','))
            |> Seq.map (fun p -> (p[0] |> int, p[1] |> int ) )
            |> Seq.toList

        let folds =
            inputParts[1].Split("\r\n")
            |> Seq.map (fun l -> l.Split('='))
            |> Seq.map (fun p -> new Fold (p[0][p[0].Length - 1], p[1] |> int ) )
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")

        new PuzzleInput (input, folds, answer)


    member private this.GetPointsText (points:(int * int) list) =
        let xMax = points |> Seq.map (fun (x,_) -> x) |> Seq.max
        let yMax = points |> Seq.map (fun (_,y) -> y) |> Seq.max

        let MakeLine yVal =
            let linePoints =
                points
                |> Seq.filter (fun (x,y) -> y = yVal)
                |> Seq.map (fun (x,_) -> x)
                |> Seq.toList

            let lineBits =
                [0..xMax]
                |> Seq.map (fun xVal -> if linePoints |> List.contains xVal then "#" else ".")

            let line = String.Join ("", lineBits)
            line


        let lines =
            [0..yMax] |> List.map MakeLine
                
        String.Join("\r\n", lines)
                

    member private this.FoldOnX (value:int) (x:int) (y:int) =
        match x with
        | _ when x <= value ->
                x, y
        | _ ->
                let newX = (value) - (x - value)
                newX, y
    
    
    member private this.FoldOnY (value:int) (x:int) (y:int) =
        match y with
        | _ when y <= value ->
                x, y
        | _ ->
                let newY = (value) - (y - value)
                x, newY


    member private this.ProcessFold (fold:Fold) (points:(int * int) list) =
        match fold.Kind with
        | 'x' ->  points |> List.map (fun (x,y) -> this.FoldOnX fold.Value x y) |> List.distinct
        | 'y' ->  points |> List.map (fun (x,y) -> this.FoldOnY fold.Value x y) |> List.distinct
        | _ -> failwith $"Unrecognized fold '{fold.Kind}'"


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let fold = puzzleData.Folds.Head

        let result =
            this.ProcessFold fold puzzleData.Input
            |> List.length

        Helper.GetPuzzleResultText ("How many dots are visible after just the first fold instruction?", (result |> string), puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =

        let rec DoIt (folds:Fold list) (points:(int * int) list) =
            match folds.Length with
            | 0 ->
                this.GetPointsText points
            | _ ->
                let newPoints = this.ProcessFold folds.Head points
                DoIt folds.Tail newPoints

        let result = DoIt puzzleData.Folds puzzleData.Input

        Helper.GetPuzzleResultText ("What code do you use to activate the infrared thermal imaging camera system?", "\r\n" + result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 13: Transparent Origami"
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput 1 "example1") )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput 1 "input") )

        yield ""
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput 2 "example1") )
        yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput 2 "input") )
        }