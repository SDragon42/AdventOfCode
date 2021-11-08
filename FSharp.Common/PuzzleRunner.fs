namespace FSharp.Common

open System
open System.Linq
open System.Reflection

type PuzzleRunner (titles: string []) =

    member private this.WriteHeader = 
        let titleWidth = 
            titles 
            |> Array.map(fun(t) -> t.Length) 
            |> Array.max
        
        let titleBorder = "+-" + String.Empty.PadRight(titleWidth, '-') + "-+"
        printfn "%s" titleBorder
        titles 
            |> Array.map(fun t -> printfn "%s" $"| {t.PadRight(titleWidth)} |") 
            |> ignore
        printfn "%s" titleBorder
        printfn ""


    member private this.WriteSectionSeparator =
        printfn ""
        printfn "%s" $"{String.Empty.PadRight(60, '-')}"
        printfn ""


    member private this.GetPuzzleTypes =
        let puzzleTypes = Assembly.GetEntryAssembly().GetTypes().Where(fun(t) -> 
            typeof<PuzzleBase>.IsAssignableFrom(t)
            )
        
        puzzleTypes.OrderByDescending(fun(t) -> t.Name).Take(1)


    member private this.SolvePuzzles =
        let puzzleTypes = this.GetPuzzleTypes

        let args:obj[] = [|true;true|]
        for i in puzzleTypes do
            let puzzle = Activator.CreateInstance(i, args) :?> PuzzleBase
            for text in puzzle.SolvePuzzle() do
                printfn "%s" text
            this.WriteSectionSeparator


    member this.Run (args: string []) =
        this.WriteHeader
        this.SolvePuzzles
        
    