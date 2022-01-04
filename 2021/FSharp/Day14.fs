// Referenced solution to solve for part 2
// https://github.com/Adidushi/AdventOfCode2021/blob/master/day%2014/solution.py

module Day14

open FSharp.Common
open System


type private PuzzleInput (input, rules, expectedAnswer) =
    inherit InputAnswer<string, int64 option> (input, expectedAnswer)
    member this.InsertionRules = rules



type Day14 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int) (name: string) =
        let day = 14

        let rawInput =
            InputHelper.LoadLines (day, name)
            |> Seq.toList

        let input = rawInput.Head
        let rules =
            rawInput |> List.skip 2
            |> List.map (fun l -> l.Split(" -> "))
            |> List.map (fun l -> l[0].ToCharArray(), l[1])
            |> List.map (fun (ca, b) -> (ca[0], ca[1]), b[0])
            |> dict

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt64

        new PuzzleInput (input, rules, answer)


    member private this.Breakup key value =
        let a, b = key
        [(a, value);(b, value)]


    member private this.ProcessPolymer2 (input:string) (rules:System.Collections.Generic.IDictionary<(char * char), char>) (numLoops:int) =
        let mutable polymer = input |> Seq.pairwise |> Seq.map (fun a -> a, 1L) |> Seq.toList |> dict
        
        for _ in [1..numLoops] do
            let mutable newPolymer = System.Collections.Generic.Dictionary<(char*char), int64>()
        
            let AddToDict (a:char) (b:char) (pair:(char * char)) =
                let key = (a, b)
                let oldCount = polymer[pair]
                if newPolymer.ContainsKey(key)
                    then newPolymer[key] <- newPolymer[key] + oldCount
                    else newPolymer.Add(key, oldCount)
        
            for pair in polymer do
                let a, b = pair.Key
                let c = rules[pair.Key]
                AddToDict a c pair.Key
                AddToDict c b pair.Key
            polymer <- newPolymer

        let elements =
            polymer |> Seq.map (fun kv -> this.Breakup kv.Key kv.Value )
            |> Seq.toList
            |> List.concat
            |> List.groupBy (fun (a, _) -> a)
            |> List.map (fun (a, b) -> a, b |> List.map (fun (x,y) -> int64 y) |> List.sum )
            |> List.sortBy (fun (_, b) -> b)
        elements


    member private this.CalcMostMinusLeast (elements:(char * int) list) =
        let _, maxE = elements[elements.Length - 1]
        let _, minE = elements[0]
        (maxE - minE)

    member private this.CalcMostMinusLeast2 (elements:(char * int64) list) =
        let _, maxE = elements[elements.Length - 1]
        let _, minE = elements[0]

        let HalfValue (value:int64) =
            let t1 = (decimal value) / (decimal 2.0)
            let t2 = Decimal.Round (t1, MidpointRounding.AwayFromZero)
            int64 t2

        let maxE2 = HalfValue maxE
        let minE2 = HalfValue minE

        maxE2 - minE2


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let result =
            this.ProcessPolymer2 puzzleData.Input puzzleData.InsertionRules 10
            |> this.CalcMostMinusLeast2
        Helper.GetPuzzleResultText ("Quantity of most common element minus the least common?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result =
            this.ProcessPolymer2 puzzleData.Input puzzleData.InsertionRules 40
            |> this.CalcMostMinusLeast2
        Helper.GetPuzzleResultText ("Quantity of most common element minus the least common?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 14: Extended Polymerization"
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput 1 "example1" ) )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput 1 "input" ) )

        yield ""
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput 2 "example1" ) )
        yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput 2 "input" ) )
        }