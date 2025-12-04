// Referenced solution to solve for part 2
// https://github.com/Adidushi/AdventOfCode2021/blob/master/day%2014/solution.py
namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open System.Collections.Generic
open Xunit



module ``Day 14: Extended Polymerization`` =
    let day = 14



    //-------------------------------------------------------------------------



    type private Puzzle () =

        member private this.Breakup key value =
            let a, b = key
            [(a, value);(b, value)]


        member private this.ProcessPolymer2 (input:string) (rules:IDictionary<(char * char), char>) (numLoops:int) =
            let mutable polymer = 
                input
                |> Seq.pairwise
                |> Seq.map (fun a -> a, 1L)
                |> Seq.toList
                |> dict
        
            for _ in [1..numLoops] do
                let mutable newPolymer = Dictionary<(char*char), int64>()
        
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
                polymer
                |> Seq.map (fun kv -> this.Breakup kv.Key kv.Value )
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


        // Quantity of most common element minus the least common?
        member this.RunPart1 (input:string) (rules:IDictionary<(char * char), char>) =
            let result =
                this.ProcessPolymer2 input rules 10
                |> this.CalcMostMinusLeast2
            result


        // Quantity of most common element minus the least common?
        member this.RunPart2 (input:string) (rules:IDictionary<(char * char), char>) =
            let result =
                this.ProcessPolymer2 input rules 40
                |> this.CalcMostMinusLeast2
            result



    //-------------------------------------------------------------------------
    
    
    
    let private GetPuzzleInput (part:int) (name:string) =
        let rawInput =
            InputHelper.LoadLines (2021, day, name)
            |> Seq.toList
        let input = rawInput.Head

        let rules =
            rawInput |> List.skip 2
            |> List.map (fun l -> l.Split(" -> "))
            |> List.map (fun l -> l[0].ToCharArray(), l[1])
            |> List.map (fun (ca, b) -> (ca[0], ca[1]), b[0])
            |> dict

        let answer = 
            InputHelper.LoadAnswer (2021, day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt64
                
        input, rules, answer
            
            
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let input, rules, expected = GetPuzzleInput 1 name
            
        let actual = (new Puzzle()).RunPart1 input rules
            
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
            
            
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, rules, expected = GetPuzzleInput 2 name
            
        let actual = (new Puzzle()).RunPart2 input rules
            
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
