module Day10

open FSharp.Common
open System


type private PuzzleInput (input, expectedAnswer) =
    inherit InputAnswer<string list, int64 option> (input, expectedAnswer)


type Day10 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)

    [<DefaultValue>]
    val mutable TagStack: char list


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 10

        let input =
            InputHelper.LoadLines (day, name)
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt64

        new PuzzleInput (input, answer)


    member private this.IsOpener (value: char) =
        match value with
        | '(' -> true
        | '[' -> true
        | '{' -> true
        | '<' -> true
        | _ -> false

    
    member private this.GetClosingTag (opening: char) =
        match opening with
        | '(' -> ')'
        | '[' -> ']'
        | '{' -> '}'
        | '<' -> '>'
        | _ -> ' '


    member private this.IsCloserFor (last: char, next: char) =
        let expectedNext = this.GetClosingTag last
        next = expectedNext


    member private this.FindSyntaxError (line: char list) =
        if line.Length = 0 then
            // Incomplete line
            this.TagStack <- [] // clear the stack
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
                    // Syntax Error line
                    this.TagStack <- [] // clear the stack
                    string next


    member private this.CompleteLine (line: char list) =
        if line.Length = 0 then
            // Incomplete line
            let result = this.TagStack
            this.TagStack <- [] // clear the stack
            result
        else
            let next = line[0]
            let remaining = line[1..]

            if this.TagStack.Length = 0 then
                this.TagStack <- [next]
                this.CompleteLine remaining
            else
                let last = this.TagStack[0]
                if this.IsCloserFor (last, next) then
                    this.TagStack <- this.TagStack[1..] // POP
                    this.CompleteLine remaining
                elif this.IsOpener next then
                    this.TagStack <- [next] @ this.TagStack // PUSH
                    this.CompleteLine remaining
                else
                    // Syntax Error line
                    this.TagStack <- [] // clear the stack
                    []


    member private this.SyntaxErrorScore (data: char) =
        match data with
        | ')' -> 3L
        | ']' -> 57L
        | '}' -> 1197L
        | '>' -> 25137L
        | _ -> 0L


    member private this.SyntaxCompletionScore (data: char) =
        match data with
        | ')' -> 1L
        | ']' -> 2L
        | '}' -> 3L
        | '>' -> 4L
        | _ -> 0L


    member private this.CalcSyntaxCompletionScore (codes: char list, currentScore: int64) =
        if codes.Length = 0 then
            currentScore
        else
            let value = this.GetClosingTag codes[0]
            let score = (currentScore * 5L) + (this.SyntaxCompletionScore value)
            this.CalcSyntaxCompletionScore (codes[1..], score)


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
            |> List.map this.SyntaxErrorScore
            |> List.sum
        Helper.GetPuzzleResultText ("What is the total syntax error score for those errors?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        this.TagStack <- []
        let incompleteLines =
            puzzleData.Input
            |> List.map (fun l -> this.CompleteLine (l |> Seq.toList))
            |> List.where (fun l -> l.Length > 0)

        let result =
            incompleteLines
            |> List.map (fun l -> this.CalcSyntaxCompletionScore (l, 0L))
            |> List.sort

        let idx = result.Length / 2

        let result = result[idx]
        Helper.GetPuzzleResultText ("What is the middle score?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 10: Syntax Scoring"
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput (1, "example1") ) )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput (1, "input") ) )
                                                                                        
        yield ""                                                                        
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput (2, "example1") ) )
        yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput (2, "input") ) )
        }