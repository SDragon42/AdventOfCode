namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 12: Passage Pathing`` =
    let day = 12



    //-------------------------------------------------------------------------



    type private Puzzle () =

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

            match position with
            | "end" ->
                [thePath]
            | _ ->
                let _, allOptions = 
                    map
                    |> List.where (fun (k,_) -> k = position)
                    |> List.exactlyOne

                let largeCaves = allOptions |> List.where Helper.IsUpper
                let smallCaves = allOptions |> List.where Helper.IsLower |> List.except thePath

                largeCaves @ smallCaves
                    |> List.map (fun p -> this.FindPaths(map, thePath, p))
                    |> List.concat


        member private this.VisitedSmallCaveTwice (thePath: string list) =
            let CountVisits (cave: string) =
                thePath
                    |> List.where (fun v -> v = cave)
                    |> List.length

            let sc =
                thePath
                |> List.where Helper.IsLower
                |> List.except ["start"]
                |> List.distinct
                |> List.map CountVisits
                |> List.distinct
                |> List.sum
            (sc > 1)


        member private this.FindPaths2 (map: (string * string list) list, thePath: string list, position: string) =
            let thePath = thePath @ [position]

            match position with
            | "end" ->
                [thePath]
            | _ ->
                let _, allOptions = 
                    map
                    |> List.where (fun (k,_) -> k = position)
                    |> List.exactlyOne

                let largeCaves = allOptions |> List.where Helper.IsUpper
                let smallCaves = 
                    if this.VisitedSmallCaveTwice(thePath)
                        then allOptions |> List.where Helper.IsLower |> List.except thePath
                        else allOptions |> List.where Helper.IsLower

                largeCaves @ smallCaves
                    |> List.except ["start"]
                    |> List.map (fun p -> this.FindPaths2(map, thePath, p))
                    |> List.concat


        // How many paths are there that visit small caves at most once?
        member this.RunPart1 (input: (string * string) list) =
            let map = this.BuildMap(input)
            let foundPaths = this.FindPaths(map, [], "start")
            let result = foundPaths.Length
            result


        // How many paths through this cave system are there?
        member this.RunPart2 (input: (string * string) list) =
            let map = this.BuildMap(input)
            let foundPaths = this.FindPaths2(map, [], "start")
            let result = foundPaths.Length
            result



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =
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
        
        input, answer
    
    
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("example2")>]
    [<InlineData("example3")>]
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
    [<InlineData("example3")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name
    
        let actual = (new Puzzle()).RunPart2 input
    
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
