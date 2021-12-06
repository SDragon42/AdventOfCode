module Day03

open FSharp.Common
open Microsoft.FSharp.Linq
open System
open System.Linq


type private PuzzleInput(input, expectedAnswer) = 
    inherit InputAnswer<List<string>, int option>(input, expectedAnswer)


type Day03 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 3

        let input = InputHelper.LoadInputFile(day, name).Split(Environment.NewLine) |> Array.toList
        
        let GetAnswer(name: string) =
            let text = InputHelper.LoadInputFile(day, $"%s{name}-answer%i{part}")
            try text |> int |> Some
            with | ex -> None
        let answer = GetAnswer(name)
        
        new PuzzleInput(input, answer)


    member private this.GetdigitsAt(position: int, input: List<string>) =
        let GetDigitAt(value: string) =
            value[position]
        input |> List.map GetDigitAt


    member private this.MostCommonBit(bits: List<char>) =
        let a = this.CountChars(bits, '0')
        let b = this.CountChars(bits, '1')
        if a = b then '1' elif a < b then '1' else '0'


    member private this.LeastCommonBit(bits: List<char>) =
        let a = this.CountChars(bits, '0')
        let b = this.CountChars(bits, '1')
        if a = b then '0' elif a < b then '0' else '1'


    member private this.CountChars(bits: List<char>, value: char) =
        let result = bits |> Seq.where (fun x -> x = value) |> Seq.toList
        result.Length


    member private this.BinToInt(value: string) =
        let rec doIt(i: int) =
            if i < 0 then
                0
            else
                let x = value[i].ToString() |> int
                let power = (value.Length - 1) - i
                let x2 = if x = 0 then 0 else pown 2 power
                x2 + doIt(i - 1)
        doIt(value.Length - 1)


    member private this.CalcPowerRatingPart(input: List<string>, func) =
        let count = input[0].Length
        let rec doIt(i: int) =
            if i < 0 then
                ""
            else
                let bits = this.GetdigitsAt(i, input)
                let x = bits |> func |> string
                doIt(i - 1) + x
        doIt(count - 1)


    member private this.FindLifeSupportRatingPart(input: List<string>, func) =
        let rec doIt(i: int, data: List<string>) = 
            if data.Length = 1 then
                data[0]
            else
                let bits = this.GetdigitsAt(i, data)
                let x = bits |> func

                let filteredData = data |> Seq.where (fun item -> item[i] = x) |> Seq.toList
                doIt(i + 1, filteredData)
        doIt(0, input)


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let gammaRate = this.CalcPowerRatingPart(puzzleData.Input, this.MostCommonBit) |> this.BinToInt
        let epsilonRate = this.CalcPowerRatingPart(puzzleData.Input, this.LeastCommonBit) |> this.BinToInt

        let result = gammaRate * epsilonRate
        Helper.GetPuzzleResultText("What is the power consumption of the submarine?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let o2GeneratorRating = this.FindLifeSupportRatingPart(puzzleData.Input, this.MostCommonBit) |> this.BinToInt
        let co2ScrubberRating = this.FindLifeSupportRatingPart(puzzleData.Input, this.LeastCommonBit) |> this.BinToInt

        let result = o2GeneratorRating * co2ScrubberRating
        Helper.GetPuzzleResultText("What is the life support rating of the submarine?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 3: Binary Diagnostic"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }