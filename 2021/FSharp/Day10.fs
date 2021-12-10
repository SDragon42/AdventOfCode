module Day10

open FSharp.Common
open System


type private PuzzleInput(input, expectedAnswer) =
    inherit InputAnswer<string list, int option>(input, expectedAnswer)


type Day10 (runBenchmarks, runExamples) =
    inherit PuzzleBase(runBenchmarks, runExamples)

    [<DefaultValue>]
    val mutable TagStack: char list


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 10

        let input =
            InputHelper.LoadLines(day, name)
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput(input, answer)


    member private this.IsOpener(value: char) =
        match value with
        | '(' -> true
        | '[' -> true
        | '{' -> true
        | '<' -> true
        | _ -> false

    
    member private this.GetClosingTag(opening: char) =
        match opening with
        | '(' -> ')'
        | '[' -> ']'
        | '{' -> '}'
        | '<' -> '>'
        | _ -> ' '


    member private this.IsCloserFor(last: char, next: char) =
        let expectedNext = this.GetClosingTag(last)
        next = expectedNext


    member private this.FindSyntaxError(line: char list) =
        if line.Length = 0 then
            ""
        else
            let next = line[0]
            let remaining = line[1..]

            if this.TagStack.Length = 0 then
                this.TagStack <- [next]
                this.FindSyntaxError remaining
            else
                let last = this.TagStack[0]
                if this.IsCloserFor (last, next) then
                    this.TagStack <- this.TagStack[1..] // POP
                    this.FindSyntaxError remaining
                elif this.IsOpener next then
                    this.TagStack <- [next] @ this.TagStack // PUSH
                    this.FindSyntaxError remaining
                else
                    string next


    member private this.TotalSyntaxErrorScore (data: char) =
        match data with
        | ')' -> 3
        | ']' -> 57
        | '}' -> 1197
        | '>' -> 25137
        | _ -> 0


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        this.TagStack <- []
        let syntaxErrors =
            puzzleData.Input
            |> List.map (fun l -> l |> Seq.toList)
            |> List.map this.FindSyntaxError
            |> List.where (fun s -> s.Length > 0)
            |> List.map char

        let result =
            syntaxErrors
            |> List.map this.TotalSyntaxErrorScore
            |> List.sum
        Helper.GetPuzzleResultText("What is the total syntax error score for those errors?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let result = 0
        Helper.GetPuzzleResultText("", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 10: Syntax Scoring"
        yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart1(this.GetPuzzleInput(1, "example1")))
        yield this.RunProblem(fun _ -> "Part 1) " + this.RunPart1(this.GetPuzzleInput(1, "input")))

        yield ""
        //yield this.RunExample(fun _ -> " Ex. 1) " + this.RunPart2(this.GetPuzzleInput(2, "example1")))
        //yield this.RunProblem(fun _ -> "Part 2) " + this.RunPart2(this.GetPuzzleInput(2, "input")))
        }