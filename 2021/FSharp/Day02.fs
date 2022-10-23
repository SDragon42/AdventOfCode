namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open Xunit



module ``Day 02: Dive!`` =
    let day = 2



    //-------------------------------------------------------------------------



    type private PilotAction =
        val Direction: string
        val Amount: int

        new(direction, amount) = { Direction = direction; Amount = amount}



    //-------------------------------------------------------------------------



    type private Puzzle () =

        member private this.CalculateSubPosition_Flawed (input: PilotAction list, (position, depth): int * int) =
            match input with
            | _ when input.Length = 0 -> 
                (position, depth)
            | _ ->
                let next = input[0]
                let remaining = input[1..]

                let values =
                    match next.Direction with
                    | "forward" ->
                        (position + next.Amount, depth)
                    | "down" ->
                        (position, depth + next.Amount)
                    | "up" ->
                        (position, depth - next.Amount)
                    | _ -> 
                        (position, depth)

                this.CalculateSubPosition_Flawed (remaining, values)


        member private this.CalculateSubPosition(input: PilotAction list, (position, depth, aim): int * int * int) =
            match input with
            | _ when input.Length = 0 ->
                (position, depth, aim)
            | _ ->
                let next = input[0]
                let remaining = input[1..]

                let values =
                    match next.Direction with
                    | "forward" -> (
                        position + next.Amount,
                        depth + (aim * next.Amount),
                        aim)
                    | "down" -> (
                        position,
                        depth,
                        aim + next.Amount)
                    | "up" -> (
                        position,
                        depth,
                        aim - next.Amount)
                    | _ -> (position, depth, aim)

                this.CalculateSubPosition(remaining, values)


        // What do you get if you multiply your final horizontal position by your final depth?
        member this.RunPart1 (input: PilotAction list) =
            let horizontal, depth = this.CalculateSubPosition_Flawed (input, (0, 0))
            let result = horizontal * depth
            result


        // What do you get if you multiply your final horizontal position by your final depth?
        member this.RunPart2 (input: PilotAction list) =
            let horizontal, depth, _ = this.CalculateSubPosition(input, (0, 0, 0))
            let result = horizontal * depth
            result



    //-------------------------------------------------------------------------



    let private GetPuzzleInput (part:int) (name:string) =

        let makePilotAction (value: string) =
            let parts = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            new PilotAction(parts[0], parts[1] |> int)

        let input = 
            InputHelper.LoadLines(day, name)
            |> Seq.map makePilotAction
            |> Seq.toList

        let answer = 
            InputHelper.LoadAnswer(day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        input, answer



    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part1 (name:string) =
        let input, expected = GetPuzzleInput 1 name

        let actual = (new Puzzle()).RunPart1 (input)

        match expected with
        | None -> Assert.Null(actual)
        | _ -> Assert.Equal(expected.Value, actual)


    [<Theory>]
    [<InlineData("example1")>]
    [<InlineData("input")>]
    let Part2 (name:string) =
        let input, expected = GetPuzzleInput 2 name

        let actual = (new Puzzle()).RunPart2 (input)

        match expected with
        | None -> Assert.Null(actual)
        | _ -> Assert.Equal(expected.Value, actual)
