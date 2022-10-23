namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open System.Collections.Generic
open Xunit



module ``Day 08: Seven Segment Search`` =
    let day = 8



    //-------------------------------------------------------------------------



    type private Puzzle () =

        member private this.IsDigit_1(pattern: string) =
            pattern.Length = 2


        member private this.IsDigit_4(pattern: string) =
            pattern.Length = 4


        member private this.IsDigit_7(pattern: string) =
            pattern.Length = 3


        member private this.IsDigit_8(pattern: string) =
            pattern.Length = 7


        member private this.ProcessDigits (data: (string list * string list)) = 
            let _, digits = data
            let count =
                (digits |> List.where this.IsDigit_1 |> List.length) +
                (digits |> List.where this.IsDigit_4 |> List.length) +
                (digits |> List.where this.IsDigit_7 |> List.length) + 
                (digits |> List.where this.IsDigit_8 |> List.length)
            count


        member private this.BuildSignalMap (signal: string list) = 
            let digitMap = new Dictionary<string, string>()

            digitMap.Add("1", signal |> List.where this.IsDigit_1 |> List.exactlyOne)
            digitMap.Add("4", signal |> List.where this.IsDigit_4 |> List.exactlyOne)
            digitMap.Add("7", signal |> List.where this.IsDigit_7 |> List.exactlyOne)
            digitMap.Add("8", signal |> List.where this.IsDigit_8 |> List.exactlyOne)

            digitMap.Add("3", 
                signal
                |> List.where (fun s -> s.Length = 5)
                |> List.where (fun l -> digitMap.Item("1") |> Seq.toList |> List.forall (fun c -> l.Contains(c)))
                |> List.exactlyOne)

            digitMap.Add("9",
                signal
                |> List.where (fun s -> s.Length = 6)
                |> List.where (fun l -> digitMap.Item("3") |> Seq.toList |> List.forall (fun c -> l.Contains(c)))
                |> List.where (fun l -> digitMap.Item("4") |> Seq.toList |> List.forall (fun c -> l.Contains(c)))
                |> List.exactlyOne)

            let char2Find = 
                (digitMap.Item("8") |> Seq.toList)
                |> List.except (digitMap.Item("9") |> Seq.toList)
                |> List.exactlyOne
            digitMap.Add("2",
                signal
                |> List.where (fun s -> s.Length = 5)
                |> List.except (digitMap.Values |> Seq.toList)
                |> List.where (fun s -> s.Contains(char2Find))
                |> List.exactlyOne)

            digitMap.Add("5",
                signal
                |> List.where (fun s -> s.Length = 5)
                |> List.except (digitMap.Values |> Seq.toList)
                |> List.exactlyOne)

            digitMap.Add("0",
                signal
                |> List.where (fun s -> s.Length = 6)
                |> List.except (digitMap.Values |> Seq.toList)
                |> List.where (fun s -> (digitMap.Item("1") |> Seq.toList) |> List.forall (fun c -> s.Contains(c)))
                |> List.exactlyOne)
        
            digitMap.Add("6",
                signal
                |> List.except (digitMap.Values |> Seq.toList)
                |> List.exactlyOne)

            let Dict2Tuple (kp: KeyValuePair<string, string>) =
                kp.Key, kp.Value |> Seq.toList

            let result = 
                digitMap
                |> Seq.map Dict2Tuple
                |> Seq.toList
            result


        // In the output values, how many times do digits 1, 4, 7, or 8 appear?
        member this.RunPart1 (input: (string list * string list) list) =
            let result = 
                input
                |> List.map this.ProcessDigits
                |> List.sum
            result


        // What do you get if you add up all of the output values?
        member this.RunPart2 (input: (string list * string list) list) =
            let ProcessLine ((signal, digits): (string list * string list)) =
                let signalMap = this.BuildSignalMap(signal)

                let GetNumber (code: string) =
                    let num, _ =
                        signalMap
                        |> List.where (fun (_, wires) -> code.Length = wires.Length)
                        |> List.where (fun (_, wires) -> wires |> List.forall (fun c -> code.Contains(c)))
                        |> List.exactlyOne
                    num
                
                String.Concat(digits |> List.map GetNumber) |> int

            let result = 
                input
                |> List.map ProcessLine
                |> List.sum
            result



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =
        let SplitData (line: string) =
            let parts = line.Split('|', 2)
            let signal = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map (fun i -> i.Trim()) |> Array.toList
            let digits = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries) |> Array.map (fun i -> i.Trim()) |> Array.toList
            signal, digits


        let input =
            InputHelper.LoadLines(day, name)
            |> Seq.map SplitData
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt
                    
        input, answer
                
                
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("example2")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let input, expected = GetPuzzleInput 1 name
                
        let actual = (new Puzzle()).RunPart1 input
                
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
                
                
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("example2")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name
                
        let actual = (new Puzzle()).RunPart2 input
                
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
