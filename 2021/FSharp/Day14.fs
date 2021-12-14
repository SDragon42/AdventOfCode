module Day14

open FSharp.Common
open System


type private PuzzleInput (input, rules, expectedAnswer) =
    inherit InputAnswer<string, int option> (input, expectedAnswer)
    member this.InsertionRules = rules



type Day14 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 14

        let rawInput =
            InputHelper.LoadLines (day, name)
            |> Seq.toList

        let input = rawInput.Head
        let rules =
            rawInput |> List.skip 2
            |> List.map (fun l -> l.Split(" -> "))
            |> List.map (fun l -> l[0], l[1])
            |> dict

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput (input, rules, answer)


    member private this.GetRule (key:string) (rules:System.Collections.Generic.IDictionary<string, string>) =
        rules[key]
        //rules
        //|> List.filter (fun (a, _) -> a = key)
        //|> List.exactlyOne
        //|> (fun (_, b) -> b)


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let rec InjectElements (polymer:string) (lastChar:string) (count:int) =
            match count with
            | 0 ->
                    polymer
            | _ ->
                    let elements = polymer.ToCharArray() |> Array.map string
                    let blah =
                        elements
                        |> Seq.pairwise
                        |> Seq.map (fun (a, b) -> a + this.GetRule (a+b) puzzleData.InsertionRules)
                        |> Seq.toList
                    let next = String.Join("", blah) + lastChar
                    InjectElements next lastChar (count - 1)

        let polymer = InjectElements puzzleData.Input (string puzzleData.Input[puzzleData.Input.Length - 1]) 10

        let elements =
            polymer.ToCharArray()
            |> Array.countBy id
            |> Array.sortBy (fun (_, b) -> b)

        let _, maxE = elements[elements.Length - 1]
        let _, minE = elements[0]

        let result = maxE - minE
            
        
        Helper.GetPuzzleResultText ("Quantity of most common element minus the least common?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        //let GetRule key =
        //    puzzleData.InsertionRules
        //    |> List.filter (fun (a, _) -> a = key)
        //    |> List.exactlyOne
        //    |> (fun (_, b) -> b)

        //let rec InjectElements (polymer:char list) (lastChar:char) (count:int) =
        //    match count with
        //    | 0 ->
        //            polymer
        //    | _ ->
        //            let blah =
        //                polymer
        //                |> Seq.pairwise
        //                |> Seq.map (fun (a, b) -> a + GetRule(a+b))
        //                |> Seq.toList
        //            let next = String.Join("", blah) + lastChar
        //            InjectElements next lastChar (count - 1)

        //let data = puzzleData.Input.ToCharArray() |> Array.toList
        //let polymer = InjectElements data (data[data.Length - 1]) 40

        //let elements =
        //    polymer.ToCharArray()
        //    |> Array.countBy id
        //    |> Array.sortBy (fun (_, b) -> b)

        //let _, maxE = elements[elements.Length - 1]
        //let _, minE = elements[0]

        //let result = maxE - minE
        let result = 0
            
        
        Helper.GetPuzzleResultText ("Quantity of most common element minus the least common?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 14: Extended Polymerization"
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput (1, "example1") ) )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput (1, "input") ) )

        yield ""
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput (2, "example1") ) )
        yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput (2, "input") ) )
        }