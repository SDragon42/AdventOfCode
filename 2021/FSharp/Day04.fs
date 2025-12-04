namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 04: Giant Squid`` =
    let day = 4



    //-------------------------------------------------------------------------



    type private BingoBoard (data: string list) =
        [<Literal>]
        let MARK = "X"
    
        let mutable boardData: string list = data
        let mutable excluded: bool = false

        member this.MarkTile(value: string) = 
            boardData <- boardData |> List.map (fun v -> if v = value then MARK else v)

        member this.MarkExcluded() =
            excluded <- true

        member this.IsExcluded() =
            excluded

        member this.CheckIfWon() =
            let GetdigitsAt(position: int, theRows: string list list) =
                let GetDigitAt(value: string list) =
                    value[position]
                theRows |> List.map GetDigitAt

            let HasLineWon(line: string list) =
                line |> List.forall (fun v -> v = MARK)

            let size = Math.Sqrt(boardData.Length) |> int
            let colRange = [0..size - 1]
            let rows = boardData |> List.splitInto(size)
            let columns = colRange |> List.map (fun i -> GetdigitsAt(i, rows))

            rows @ columns
            |> List.map HasLineWon
            |> List.exists (fun l -> l)

        member this.SumUnmarkedNumbers() =
            boardData
            |> List.where (fun x -> x <> MARK)
            |> List.map int
            |> List.sum
                


    //-------------------------------------------------------------------------



    type private Puzzle () =

        // What will your final score be?
        member this.RunPart1 (input: string list) (boards: BingoBoard list) =
            let rec Play (numberDraw: string) (remaining: string list) =
                boards
                |> List.map (fun b -> b.MarkTile(numberDraw))
                |> ignore

                let winningBoards =
                    boards 
                    |> List.where (fun b -> b.CheckIfWon())

                match winningBoards.Length with
                | 0 -> Play remaining[0] remaining[1..]
                | _ -> (winningBoards[0], numberDraw)

            let winningBoard, winningDraw = Play input[0] input[1..]

            winningBoard.SumUnmarkedNumbers() * int(winningDraw)


        // What would its final score be?
        member this.RunPart2 (input: string list) (boards: BingoBoard list) =
            let rec Play (numberDraw: string) (remaining: string list) =
                let numWonBoards = 
                    boards 
                    |> List.where (fun b -> b.IsExcluded())
                    |> List.length

                boards 
                    |> List.where (fun b -> b.IsExcluded() = false)
                    |> List.map (fun b -> b.MarkTile(numberDraw))
                    |> ignore

                let winningBoards =
                    boards
                    |> List.where (fun b -> b.IsExcluded() = false)
                    |> List.where (fun b -> b.CheckIfWon())

                winningBoards
                |> List.map (fun b -> b.MarkExcluded())
                |> ignore

                match winningBoards with
                | _ when winningBoards.Length = 0 -> 
                    Play remaining[0] remaining[1..]
                | _ when numWonBoards + 1 < boards.Length -> 
                    Play remaining[0] remaining[1..]
                | _ -> 
                    winningBoards[0], numberDraw

            let winningBoard, winningDraw = Play input[0] input[1..]

            winningBoard.SumUnmarkedNumbers() * int(winningDraw)



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =
        let tempInput =
            InputHelper.LoadText(2021, day, name).Split(Environment.NewLine+Environment.NewLine)
            |> Array.toList

        let input =
            tempInput[0].Split(',')
            |> Seq.toList

        let splitValue = [|Environment.NewLine; " "|]
        let boardsData = 
            tempInput[1..]
            |> List.map (fun b -> b.Split(splitValue, StringSplitOptions.RemoveEmptyEntries) |> Array.toList)
            |> List.map (fun b -> new BingoBoard(b))
        
        let answer = 
            InputHelper.LoadAnswer (2021, day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt
            
        input, answer, boardsData
        
        
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let input, expected, boardsData = GetPuzzleInput 1 name
        
        let actual = (new Puzzle()).RunPart1 input boardsData
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
        
        
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected, boardsData = GetPuzzleInput 2 name
        
        let actual = (new Puzzle()).RunPart2 input boardsData
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
