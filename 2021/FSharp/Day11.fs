module Day11

open FSharp.Common
open System


type private PuzzleInput (input, xMax: int, yMax: int, expectedAnswer) =
    inherit InputAnswer<int list, int option> (input, expectedAnswer)
    member this.xMax = xMax
    member this.yMax = yMax


type Day11 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)

    [<DefaultValue>]
    val mutable grid: int list
    [<DefaultValue>]
    val mutable xMax: int
    [<DefaultValue>]
    val mutable yMax: int


    member private this.GetPuzzleInput (part: int, name: string) =
        let day = 11

        let rawinput =
            InputHelper.LoadLines (day, name)
            |> Seq.toList
        let xMax = rawinput[0].Length
        let yMax = rawinput.Length

        let input =
            rawinput
            |> List.map (fun s -> s |> Seq.toList |> List.map (fun c -> c |> string |> int) )
            |> List.concat


        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput (input, xMax, yMax, answer)


    member private this.PowerUp (value: int, amount: int) =
        if value >= 10 then
            value
        else
            let newValue = value + amount
            if newValue <= 10 
            then newValue 
            else 10


    member private this.SuperSaiyan (value: int) =
        if value = 10 then 11 else value


    member private this.FlashBurnout (value: int) =
        if value > 9 then 0 else value


    member private this.DoFlashPowerUp () =
        let flashedIndexs =
            this.grid |> List.mapi (fun i v -> (i, v))
            |> List.where (fun (_, v) -> v = 10)
            |> List.map (fun (i, _) -> GridHelper.GetAdjacentIndexes(i, this.xMax, this.grid.Length, GridHelper.Offsets8))
            |> List.concat
            |> List.sort
            |> List.groupBy (fun k -> k)
            |> List.map (fun (i, l) -> (i, l.Length))

        let FlashedPowerUp (idx: int, value: int) =
            let flashCount =
                flashedIndexs |> List.where (fun (i, v) -> i = idx)
                |> List.map (fun (i, v) -> v)
                |> List.sum

            if flashCount > 0
                then this.PowerUp (value, flashCount)
                else value

        this.grid <- this.grid |> List.map this.SuperSaiyan
        this.grid <- this.grid |> List.mapi (fun i v -> FlashedPowerUp(i, v))
               
        let any2Flash = this.grid |> List.exists (fun v -> v = 10)
        if any2Flash
            then this.DoFlashPowerUp()
            else ignore


    member private this.RunGridStep () =
        this.grid <- this.grid |> List.map (fun v -> this.PowerUp(v, 1))
        this.DoFlashPowerUp() |> ignore
        this.grid <- this.grid |> List.map this.FlashBurnout


    member private this.ShowGrid (title: string) =
        let NumToString (value) =
            match value with
            | _ when value >= 0 && value <= 9 -> string value
            | 10 -> "*"
            | _ -> "."

        printfn "%s" title
        for y in [0..this.yMax] do
            let idxStart = y * this.xMax
            let idxEnd = idxStart + (this.xMax - 1)
            let digits = this.grid[idxStart..idxEnd] |> List.map NumToString
            let line = String.Join ("", digits)
            printfn "%s" line
        printfn ""
                


    member private this.SetClassVars (puzzleData: PuzzleInput) =
        this.grid <- puzzleData.Input
        this.xMax <- puzzleData.xMax
        this.yMax <- puzzleData.yMax


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        this.SetClassVars puzzleData

        let rec DoIt (step: int) =
            this.RunGridStep ()

            let numberFlashes = this.grid |> List.where (fun v -> v = 0) |> List.length
            numberFlashes

        let result =
            [1..100]
            |> List.map DoIt
            |> List.sum
            
        Helper.GetPuzzleResultText ("How many total flashes are there after 100 steps?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        this.SetClassVars puzzleData

        let rec DoIt (step: int) =
            this.RunGridStep ()

            let allFlashed = this.grid |> List.forall (fun v -> v = 0)
            if allFlashed = false
                then DoIt(step + 1)
                else step

        let result = DoIt(1)

        Helper.GetPuzzleResultText ("What is the first step during which all octopuses flash?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 11: Dumbo Octopus"
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput (1, "example1") ) )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput (1, "input") ) )

        yield ""
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput (2, "example1") ) )
        yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput (2, "input") ) )
        }