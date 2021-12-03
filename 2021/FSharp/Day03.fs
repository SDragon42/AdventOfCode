module Day03

open FSharp.Common
open Microsoft.FSharp.Linq
open System
open System.Linq


type private PuzzleInput(input: string[], expectedAnswer: int option) = 
    inherit InputAnswer<string [], int option>(input, expectedAnswer)


type Day03 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 3

        let input = InputHelper.LoadInputFile(day, name).Split(Environment.NewLine) //|> Seq.map int |> Seq.toArray
        
        let GetAnswer(name: string) =
            let text = InputHelper.LoadInputFile(day, $"%s{name}-answer%i{part}")
            try
                text |> int |> Some
            with
                | ex -> None
        let answer = GetAnswer(name)
        
        new PuzzleInput(input, answer)


    member private this.GetdigitsAt(position: int, input: string[]) =
        let GetDigitAt(value: string) =
            value[position]
        input |> Array.map GetDigitAt


    member private this.MostCommonBit(bits: char[]) =
        let a = this.CountChars(bits, '0')
        let b = this.CountChars(bits, '1')
        if a = b then '1' elif a < b then '1' else '0'


    member private this.LeastCommonBit(bits: char[]) =
        let a = this.CountChars(bits, '0')
        let b = this.CountChars(bits, '1')
        if a = b then '0' elif a < b then '0' else '1'


    member private this.CountChars(bits: char[], value: char) =
        let result = bits |> Seq.where (fun x -> x = value) |> Seq.toArray
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


    member private this.CalculateGammaRate(input: string[]) =
        let count = input[0].Length
        let rec doIt(i: int) =
            if i < 0 then
                ""
            else
                let bits = this.GetdigitsAt(i, input)
                let x = this.MostCommonBit(bits).ToString()
                doIt(i - 1) + x
        doIt(count - 1)
        

    member private this.CalculateEpsilonRate(input: string[]) =
        let count = input[0].Length
        let rec doIt(i: int) =
            if i < 0 then
                ""
            else
                let bits = this.GetdigitsAt(i, input)
                let x = this.LeastCommonBit(bits).ToString()
                doIt(i - 1) + x
        doIt(count - 1)


    member private this.FindO2GeneratorRating(input: string[]) =
        let rec doIt(i: int, data: string[]) = 
            if data.Length = 1 then
                data[0]
            else
                let bits = this.GetdigitsAt(i, data)
                let x = this.MostCommonBit(bits)

                let filteredData = data |> Seq.where (fun item -> item[i] = x) |> Seq.toArray
                doIt(i + 1, filteredData)
        doIt(0, input)


    member private this.FindCO2ScrubberRating(input: string[]) =
        let rec doIt(i: int, data: string[]) = 
            if data.Length = 1 then
                data[0]
            else
                let bits = this.GetdigitsAt(i, data)
                let x = this.LeastCommonBit(bits)

                let filteredData = data |> Seq.where (fun item -> item[i] = x) |> Seq.toArray
                doIt(i + 1, filteredData)
        doIt(0, input)


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let gammaRate = this.CalculateGammaRate(puzzleData.Input) |> this.BinToInt
        let epsilonRate = this.CalculateEpsilonRate(puzzleData.Input) |> this.BinToInt

        let result = gammaRate * epsilonRate
        Helper.GetPuzzleResultText("What is the power consumption of the submarine?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let o2GeneratorRating = this.FindO2GeneratorRating(puzzleData.Input) |> this.BinToInt
        let co2ScrubberRating = this.FindCO2ScrubberRating(puzzleData.Input) |> this.BinToInt

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