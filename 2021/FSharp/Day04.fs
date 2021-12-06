module Day04

open FSharp.Common
open Microsoft.FSharp.Linq
open System
open System.Linq


type BingoBoard (data: List<string>) =
    [<Literal>]
    let MARK = "X"

    let mutable boardData: List<string> = data

    let mutable excluded: bool = false

    member this.MarkTile(value: string) = 
        boardData <- boardData |> List.map (fun v -> if v = value then MARK else v)

    member this.MarkExcluded() =
        excluded <- true

    member this.IsExcluded() =
        excluded

    member this.CheckIfWon() =
        let GetdigitsAt(position: int, theRows: List<List<string>>) =
            let GetDigitAt(value: List<string>) =
                value[position]
            theRows |> List.map GetDigitAt

        let HasLineWon(line: List<string>) =
            line |> List.forall (fun v -> v = MARK)

        let size = Math.Sqrt(boardData.Length) |> int
        let colRange = [0..size - 1]
        let rows = boardData |> List.splitInto(size)
        let columns =  colRange |> List.map (fun i -> GetdigitsAt(i, rows))

        rows @ columns |> List.map HasLineWon |> List.exists (fun l -> l)

    member this.SumUnmarkedNumbers() =
        boardData |> List.where (fun x -> x <> MARK) |> List.map int |> List.sum
                


type private PuzzleInput(input, expectedAnswer, boards: List<BingoBoard>) = 
    inherit InputAnswer<List<string>, int option>(input, expectedAnswer)

    member this.Boards = boards



type Day04 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 4

        let input = InputHelper.LoadInputFile(day, name).Split(Environment.NewLine+Environment.NewLine) |> Array.toList

        let numberDraw = input[0].Split(',') |> Seq.toList //|> List.map int

        let splitValue = [Environment.NewLine; " "] |> List.toArray
        let boardsData = 
            input[1..]
            |> List.map (fun b -> b.Split(splitValue, StringSplitOptions.RemoveEmptyEntries) |> Array.toList)
            |> List.map (fun b -> new BingoBoard(b))

        let GetAnswer(name: string) =
            let text = InputHelper.LoadInputFile(day, $"%s{name}-answer%i{part}")
            try text |> int |> Some
            with | ex -> None
        let answer = GetAnswer(name)
        
        new PuzzleInput(numberDraw, answer, boardsData)


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let rec Play(numberDraw: string, remaining: List<string>) =
            puzzleData.Boards |> List.map (fun b -> b.MarkTile(numberDraw)) |> ignore

            let winningBoards =
                puzzleData.Boards 
                |> List.where (fun b -> b.CheckIfWon())

            if (winningBoards.Length = 0)
                then Play(remaining[0], remaining[1..])
                else winningBoards[0], numberDraw

        let winningBoard, winningDraw = Play(puzzleData.Input[0], puzzleData.Input[1..])

        let result = winningBoard.SumUnmarkedNumbers() * int(winningDraw)
        Helper.GetPuzzleResultText("What will your final score be?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let rec Play(numberDraw: string, remaining: List<string>) =
            let numWonBoards = 
                puzzleData.Boards 
                |> List.where (fun b -> b.IsExcluded())
                |> List.length

            puzzleData.Boards 
                |> List.where (fun b -> b.IsExcluded() = false)
                |> List.map (fun b -> b.MarkTile(numberDraw)) |> ignore

            let winningBoards =
                puzzleData.Boards
                |> List.where (fun b -> b.IsExcluded() = false)
                |> List.where (fun b -> b.CheckIfWon())

            winningBoards |> List.map (fun b -> b.MarkExcluded()) |> ignore

            if winningBoards.Length = 0 || numWonBoards + 1 < puzzleData.Boards.Length then
                Play(remaining[0], remaining[1..])
            else
                winningBoards[0], numberDraw

        let winningBoard, winningDraw = Play(puzzleData.Input[0], puzzleData.Input[1..])

        let result = winningBoard.SumUnmarkedNumbers() * int(winningDraw)
        Helper.GetPuzzleResultText("What would its final score be?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 4: Giant Squid"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }