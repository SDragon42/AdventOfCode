module Day11

open FSharp.Common
open System


type private PuzzleInput (input, xMax: int, yMax: int, expectedAnswer) =
    inherit InputAnswer<int list, int option> (input, expectedAnswer)
    member this.xMax = xMax
    member this.yMax = yMax


type Day11 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)


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

    //member private this.offsets = [
    //    ( 0,-1); ( 1,-1);
    //    ( 1, 0); ( 1, 1);
    //    ( 0, 1); (-1, 1);
    //    (-1, 0); (-1, 1);
    //    ]

    //val private this.xMax: int

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


    member private this.ShowGrid (title: string, grid: int list, xMax: int, yMax: int) =
        let NumToString (value) =
            match value with
            | _ when value >= 0 && value <= 9 -> string value
            | 10 -> "*"
            | _ -> "."


        printfn "%s" title
        for y in [0..yMax] do
            let idxStart = y * xMax
            let idxEnd = idxStart + (xMax - 1)
            let digits = grid[idxStart..idxEnd] |> List.map NumToString //List.map string
            let line = String.Join ("", digits)
            printfn "%s" line
        printfn ""
                


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let stepsList = [1..100]
        let mutable grid = puzzleData.Input

        //this.ShowGrid("Initial", grid, puzzleData.xMax, puzzleData.yMax)


        let rec DoFlashPowerUp () =
            //grid <- grid |> List.map this.SuperSaiyan

            //let any10s = grid |> List.where (fun v -> v = 10) |> List.length // DEBUGGING

            let flashedIndexs =
                grid |> List.mapi (fun i v -> (i, v))
                |> List.where (fun (_, v) -> v = 10)
                |> List.map (fun (i, _) -> GridHelper.GetAdjacentIndexes(i, puzzleData.xMax, grid.Length, GridHelper.Offsets8))
                |> List.concat
                |> List.sort
                |> List.groupBy (fun k -> k)
                |> List.map (fun (i, l) -> (i, l.Length))
                //|> List.distinct

            let FlashedPowerUp (idx: int, value: int) =
                //let isFlashIdx = flashedIndexs |> List.contains idx
                let flashAmmount =
                    flashedIndexs |> List.where (fun (i, v) -> i = idx)
                    |> List.map (fun (i, v) -> v)
                    |> List.sum

                if flashAmmount = 0 then
                    value
                else
                    this.PowerUp (value, flashAmmount)

            grid <- grid |> List.map this.SuperSaiyan
            grid <- grid |> List.mapi (fun i v -> FlashedPowerUp(i, v))
            
            //flashedIndexs

            let any2Flash = grid |> List.exists (fun v -> v = 10)

            if any2Flash
                then DoFlashPowerUp()
                else ignore


        let rec DoIt (step: int) =
            grid <- grid |> List.map (fun v -> this.PowerUp(v, 1)) // regular power level increase
            //this.ShowGrid ("Natural PowerUp", grid, puzzleData.xMax, puzzleData.yMax)
            DoFlashPowerUp() |> ignore
            //this.ShowGrid ("Post Flash PowerUp", grid, puzzleData.xMax, puzzleData.yMax)
            grid <- grid |> List.map this.FlashBurnout

            //this.ShowGrid ($"After step %i{step}:", grid, puzzleData.xMax, puzzleData.yMax)

            let numberFlashes = grid |> List.where (fun v -> v = 0) |> List.length
            numberFlashes

        let result =
            stepsList |> List.map DoIt //(fun step -> DoIt (step, grid))
            |> List.sum
            
        
        //let result = DoIt puzzleData.Input
        Helper.GetPuzzleResultText ("How many total flashes are there after 100 steps?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let stepsList = [1..100]
        let mutable grid = puzzleData.Input

        //this.ShowGrid("Initial", grid, puzzleData.xMax, puzzleData.yMax)


        let rec DoFlashPowerUp () =
            //grid <- grid |> List.map this.SuperSaiyan

            //let any10s = grid |> List.where (fun v -> v = 10) |> List.length // DEBUGGING

            let flashedIndexs =
                grid |> List.mapi (fun i v -> (i, v))
                |> List.where (fun (_, v) -> v = 10)
                |> List.map (fun (i, _) -> GridHelper.GetAdjacentIndexes(i, puzzleData.xMax, grid.Length, GridHelper.Offsets8))
                |> List.concat
                |> List.sort
                |> List.groupBy (fun k -> k)
                |> List.map (fun (i, l) -> (i, l.Length))
                //|> List.distinct

            let FlashedPowerUp (idx: int, value: int) =
                //let isFlashIdx = flashedIndexs |> List.contains idx
                let flashAmmount =
                    flashedIndexs |> List.where (fun (i, v) -> i = idx)
                    |> List.map (fun (i, v) -> v)
                    |> List.sum

                if flashAmmount = 0 then
                    value
                else
                    this.PowerUp (value, flashAmmount)

            grid <- grid |> List.map this.SuperSaiyan
            grid <- grid |> List.mapi (fun i v -> FlashedPowerUp(i, v))
            
            //flashedIndexs

            let any2Flash = grid |> List.exists (fun v -> v = 10)

            if any2Flash
                then DoFlashPowerUp()
                else ignore


        let rec DoIt (step: int) =
            grid <- grid |> List.map (fun v -> this.PowerUp(v, 1))
            DoFlashPowerUp() |> ignore
            grid <- grid |> List.map this.FlashBurnout

            let allFlashed = grid |> List.forall (fun v -> v = 0)
            if allFlashed = false
                then DoIt(step + 1)
                else step


        let result = DoIt(1)

        Helper.GetPuzzleResultText ("What is the first step during which all octopuses flash?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 11: Dumbo Octopus"
        //yield this.RunExample (fun _ -> " Ex. 0) " + this.RunPart1 (this.GetPuzzleInput (1, "example0") ) )
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput (1, "example1") ) )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput (1, "input") ) )

        yield ""
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput (2, "example1") ) )
        yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput (2, "input") ) )
        }