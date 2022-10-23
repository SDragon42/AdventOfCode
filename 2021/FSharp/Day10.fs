namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 10: Syntax Scoring`` =
    let day = 10


    //-------------------------------------------------------------------------



    type private Puzzle () =

        [<DefaultValue>]
        val mutable TagStack: char list


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


        // What is the total syntax error score for those errors?
        member this.RunPart1 (input: string list) =
            this.TagStack <- []
            let syntaxErrors =
                input
                |> List.map (fun l -> l |> Seq.toList)
                |> List.map this.FindSyntaxError
                |> List.where (fun s -> s.Length > 0)
                |> List.map char

            let result =
                syntaxErrors
                |> List.map this.SyntaxErrorScore
                |> List.sum
            result


        // What is the middle score?
        member this.RunPart2 (input: string list) =
            this.TagStack <- []
            let incompleteLines =
                input
                |> List.map (fun l -> this.CompleteLine (l |> Seq.toList))
                |> List.where (fun l -> l.Length > 0)

            let result =
                incompleteLines
                |> List.map (fun l -> this.CalcSyntaxCompletionScore (l, 0L))
                |> List.sort

            let idx = result.Length / 2

            let result = result[idx]
            result



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =
        let input = 
            InputHelper.LoadLines (day, name)
            |> Seq.toList
        
        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt64
            
        input, answer
        
        
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let input, expected = GetPuzzleInput 1 name
        
        let actual = (new Puzzle()).RunPart1 input
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
        
        
    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name
        
        let actual = (new Puzzle()).RunPart2 input
        
        match expected with
        | None -> Assert.Null actual
        | _ -> Assert.Equal (expected.Value, actual)
