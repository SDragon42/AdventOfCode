module Day12

open FSharp.Common
open System


type private PuzzleInput (input, expectedAnswer) =
    inherit InputAnswer<(string * string) list, int option> (input, expectedAnswer)


type Day12 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 12

        let SplitPassage (line: string) =
            let parts = line.Split('-')
            parts[0], parts[1]

        let input =
            InputHelper.LoadLines (day, name)
            |> Seq.map SplitPassage
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput (input, answer)


    //member private this.IsUpper(value: string) =
    //    value = value.ToUpper()


    member private this.BuildMap (input: (string * string) list) =
        let reversedInput = input |> List.map (fun (a, b) -> b, a)
        let fullInput = input @ reversedInput
        let keys = fullInput |> List.map (fun (a, b) -> a) |> List.distinct |> List.sort

        let GetDestinations (key: string) =
            let values =
                fullInput
                |> List.where (fun (k, _) -> k = key)
                |> List.map (fun (_, v) -> v)
            key, values

        let output = keys |> List.map GetDestinations
        output


    member private this.FindPaths (map: (string * string list) list, thePath: string list, position: string) =
        let thePath = thePath @ [position]

        if position = "end" then
            [thePath]
        else
            let _, allOptions = 
                map
                |> List.where (fun (k,_) -> k = position)
                |> List.exactlyOne

            let largeCaves = allOptions |> List.where Helper.IsUpper
            let smallCaves = allOptions |> List.where Helper.IsLower |> List.except thePath
            let viablePaths = largeCaves @ smallCaves
            let aa =
                viablePaths
                |> List.map (fun p -> this.FindPaths(map, thePath, p))
                |> List.concat
            aa

    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let map = this.BuildMap(puzzleData.Input)

        let foundPaths = this.FindPaths(map, [], "start")
        
        let result = foundPaths.Length
        Helper.GetPuzzleResultText ("How many paths are there that visit small caves at most once?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText ("", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 12: Passage Pathing"
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput (1, "example1") ) )
        yield this.RunExample (fun _ -> " Ex. 2) " + this.RunPart1 (this.GetPuzzleInput (1, "example2") ) )
        yield this.RunExample (fun _ -> " Ex. 3) " + this.RunPart1 (this.GetPuzzleInput (1, "example3") ) )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput (1, "input") ) )

        yield ""
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput (2, "example1") ) )
        //yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput (2, "input") ) )
        }