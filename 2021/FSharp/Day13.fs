namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 13: Transparent Origami`` =
    let day = 13



    //-------------------------------------------------------------------------



    type private Fold (kind, value) =
        member this.Kind = kind
        member this.Value = value



    type private Puzzle () =

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
                
            String.Join("\n", lines)
                

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


        // How many dots are visible after just the first fold instruction?
        member this.RunPart1 (input: (int * int) list) (folds: Fold list) =
            let fold = folds.Head

            let result =
                this.ProcessFold fold input
                |> List.length
                |> string
            result


        // What code do you use to activate the infrared thermal imaging camera system?
        member this.RunPart2 (input: (int * int) list) (folds: Fold list) =

            let rec DoIt (folds:Fold list) (points:(int * int) list) =
                match folds.Length with
                | 0 ->
                    this.GetPointsText points
                | _ ->
                    let newPoints = this.ProcessFold folds.Head points
                    DoIt folds.Tail newPoints

            let result = DoIt folds input
            result


        
    //-------------------------------------------------------------------------
        
        
        
    let private GetPuzzleInput (part:int) (name:string) =
        let inputParts = InputHelper.LoadText(day, name).Split("\n\n")
        
        let input =
            inputParts[0].Split("\n")
            |> Seq.map (fun l -> l.Split(','))
            |> Seq.map (fun p -> (p[0] |> int, p[1] |> int ) )
            |> Seq.toList
        
        let folds =
            inputParts[1].Split("\n")
            |> Seq.map (fun l -> l.Split('='))
            |> Seq.map (fun p -> new Fold (p[0][p[0].Length - 1], p[1] |> int ) )
            |> Seq.toList
        
        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
                    
        input, folds, answer
                
                
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let input, folds, expected = GetPuzzleInput 1 name
                
        let actual = (new Puzzle()).RunPart1 input folds
                
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
                
                
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, folds, expected = GetPuzzleInput 2 name
                
        let actual = (new Puzzle()).RunPart2 input folds
                
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
                