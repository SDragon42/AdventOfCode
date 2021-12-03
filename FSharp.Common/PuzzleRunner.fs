namespace FSharp.Common

open System
open System.Linq
open System.Reflection
open CommandLine

type PuzzleRunner (titles: string []) =
    let titleLines = titles

    member private this.WriteHeader =
        let titleWidth =
            titleLines
            |> Array.map(fun(t) -> t.Length)
            |> Array.max

        let titleBorder = "+-" + String.Empty.PadRight(titleWidth, '-') + "-+"
        printfn "%s" titleBorder
        titleLines
            |> Array.map(fun t -> printfn "%s" $"| {t.PadRight(titleWidth)} |")
            |> ignore
        printfn "%s" titleBorder
        printfn ""


    member private this.WriteSectionSeparator =
        printfn ""
        printfn "%s" $"{String.Empty.PadRight(60, '-')}"
        printfn ""


    member private this.GetPuzzleTypes (options: CmdLineOptions) =
        let puzzleTypes =
            Assembly.GetEntryAssembly().GetTypes()
            |> Seq.where (fun t -> typeof<PuzzleBase>.IsAssignableFrom(t))
            |> Seq.where (fun t -> t.Name.StartsWith("Day"))

        let FindMatches =
            match options with
            | _ when options.RunAll ->
                    puzzleTypes
            | _ when options.PuzzleDays.Count() = 0 ->
                    puzzleTypes.OrderByDescending(fun(t) -> t.Name) |> Seq.take(1)
            | _ ->
                    let puzzleNames = options.PuzzleDays |> Seq.map(fun i -> $"Day%02i{i}")
                    puzzleTypes.Where(fun t -> puzzleNames.Any(fun n -> t.Name.StartsWith(n)))

        FindMatches |> Seq.toList


    member private this.SolvePuzzles (options: CmdLineOptions) =
        let puzzleTypes = this.GetPuzzleTypes(options)

        let args:obj[] = [| options.RunBenchmark ; options.RunExamples|]
        for i in puzzleTypes do
            let puzzle = Activator.CreateInstance(i, args) :?> PuzzleBase
            for text in puzzle.SolvePuzzle() do
                if (text <> null) then
                    printfn "%s" text
            this.WriteSectionSeparator


    member this.Run (args: string []) =
        this.WriteHeader
        Parser.Default.ParseArguments<CmdLineOptions>(args).WithParsed(fun o -> this.SolvePuzzles(o))
            |> ignore
