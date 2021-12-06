module Day05

open FSharp.Common
open System


type private PuzzleInput(input, expectedAnswer) =
    inherit InputAnswer<string list, int option>(input, expectedAnswer)


type Day05 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 5

        let input = 
            InputHelper.LoadLines(day, name)
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput(input, answer)


    member private this.ParseToEndPointStrings (value: string) =
        value.Split(" -> ") |> Array.toList

    member private this.ParseStringEndpointToIntList (l: string list) =
        l |> List.map (fun a -> a.Split(",") |> Array.map int |> Array.toList)

    member private this.OnlyFlatLines (z: int list list) =
        z[0][0] = z[1][0] || z[0][1] = z[1][1]

    member private this.MakeRanges (z: int list list) =
        let x1 = z[0][0]
        let y1 = z[0][1]
        let x2 = z[1][0]
        let y2 = z[1][1]
        let xStep = if x1 <= x2 then 1 else -1
        let yStep = if y1 <= y2 then 1 else -1
        
        let rX = [x1..xStep..x2]
        let rY = [y1..yStep..y2]
        rX, rY

    member private this.MakePoints (value: int list * int list) =
        let a, b = value
        if a.Length <> b.Length then
            List.allPairs a b
        else
            [0..a.Length - 1]
            |> List.map (fun i -> [a[i], b[i]])
            |> List.concat
            

    member private this.ToPointString (value: int * int) =
        let a, b = value
        $"{a},{b}"


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let coordinates = 
            puzzleData.Input |> List.map this.ParseToEndPointStrings
            |> List.map this.ParseStringEndpointToIntList
            |> List.where this.OnlyFlatLines
            |> List.map this.MakeRanges
            |> List.map this.MakePoints
            |> List.concat
            |> List.map this.ToPointString
        //coordinates |> List.map (fun p -> printfn "%s" p) |> ignore
        let uCoordinates = coordinates |> List.distinct
        let result = 
            uCoordinates 
            |> List.map (fun p -> coordinates |> List.where (fun z -> z = p) |> List.length > 1)
            |> List.where (fun i -> i) 
            |> List.length
        Helper.GetPuzzleResultText("At how many points do at least two lines overlap?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let coordinates = 
            puzzleData.Input |> List.map this.ParseToEndPointStrings
            |> List.map this.ParseStringEndpointToIntList
            |> List.map this.MakeRanges
            |> List.map this.MakePoints
            |> List.concat
            |> List.map this.ToPointString
        //coordinates |> List.map (fun p -> printfn "%s" p) |> ignore
        let uCoordinates = coordinates |> List.distinct
        let result = 
            uCoordinates 
            |> List.map (fun p -> coordinates |> List.where (fun z -> z = p) |> List.length > 1)
            |> List.where (fun i -> i) 
            |> List.length
        Helper.GetPuzzleResultText("At how many points do at least two lines overlap?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 5: Hydrothermal Venture"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }