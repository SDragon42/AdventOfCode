module Day17

open FSharp.Common
open System
open System.Text.RegularExpressions

// It took along time for me to realize part 1 required no code... 
// The answer is just n(n + 1)/2 where n = -target_min_y - 1.


type Area (xLow:int, xHigh:int, yLow:int, yHigh:int) =
    member this.XLow = xLow
    member this.XHigh = xHigh
    member this.YLow = yLow
    member this.YHigh = yHigh

    member this.InRange (x:int, y:int) =
        match x,y with
        | _ when x >= this.XLow && x <= this.XHigh && y >= this.YLow && y <= this.YHigh -> true
        | _ -> false

    member this.PassedRange (x:int, y:int) =
        match x,y with
        | _ when y < this.YLow -> true
        | _ when (abs x) > (abs this.XHigh) -> true
        | _ -> false


type private PuzzleInput (input, expectedAnswer) =
    inherit InputAnswer<Area, int option> (input, expectedAnswer)


type Day17 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part:int) (name: string) =
        let day = 17
        let lineRegEx = "x=([0-9]+)..([0-9]+), y=([-]*[0-9]+)..([-]*[0-9]+)"

        let input =
            InputHelper.LoadText (day, name)
            |> (fun t -> Regex.Match (t, lineRegEx))
            |> (fun m -> m.Groups)
            |> Seq.skip 1
            |> Seq.map (fun g -> int g.Value)
            |> Seq.toList
            |> (fun ln -> Area(ln[0], ln[1], ln[2], ln[3]))

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt

        new PuzzleInput (input, answer)


    member private this.FindMinXVelocity (target:int) (velocity:int) =
        let value = [0..velocity] |> List.sum
        match value with
        | _ when value >= target -> velocity
        | _ -> this.FindMinXVelocity target (velocity + 1)


    member private this.FindMaxYVelocity (target:int) =
        abs target - 1


    member private this.ProcessStep (target:Area) (pX:int, pY:int) (velX:int, velY:int) =
        let ShiftVelocity (velX:int, velY:int) =
            let y = velY - 1
            let x =
                match velX with
                | _ when velX > 0 -> velX - 1
                | _ when velX < 0 -> velX + 1
                | _ -> 0
            x, y

        let shiftedPos = (pX + velX) , (pY + velY)
        let shiftedVel = ShiftVelocity (velX, velY)
        
        match shiftedPos with
        | _ when target.InRange(shiftedPos) ->
                true, [shiftedPos]
        | _ when target.PassedRange(shiftedPos) ->
                false, [shiftedPos]
        | _ ->
                let result, pos = this.ProcessStep target shiftedPos shiftedVel
                result, [shiftedPos] @ pos
    

    member private this.InTargetArea (target:Area) (initialVelocity:(int * int)) =
        let success, _ = this.ProcessStep target (0,0) initialVelocity
        success, initialVelocity


    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let minXVelocity = this.FindMinXVelocity puzzleData.Input.XLow 0
        let maxYVelocity = this.FindMaxYVelocity puzzleData.Input.YLow

        let initalVelocity = (minXVelocity, maxYVelocity)
        let success, points = this.ProcessStep puzzleData.Input (0,0) initalVelocity
        
        let result =
            match success with
            | true -> points |> List.map (fun (x,y) -> y) |> List.max
            | false -> -9999
            
        Helper.GetPuzzleResultText ("What is the highest y position it reaches on this trajectory?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let minXVelocity = this.FindMinXVelocity puzzleData.Input.XLow 0
        let maxXVelocity = puzzleData.Input.XHigh
        let minYVelocity = puzzleData.Input.YLow
        let maxYVelocity = this.FindMaxYVelocity puzzleData.Input.YLow

        let xRange = [minXVelocity..maxXVelocity]
        let yRange = [minYVelocity..maxYVelocity]

        let allVelocities = 
            yRange
            |> List.map (fun y -> xRange |> List.map (fun x -> (x,y)))
            |> List.concat
            |> List.map (fun vel -> this.InTargetArea puzzleData.Input vel)
            |> List.filter (fun (success, _) -> success)
            |> List.map (fun (_, vel) -> vel)
            |> List.distinct


        let result = allVelocities.Length
        Helper.GetPuzzleResultText ("How many distinct initial velocity values cause the probe to be within the target area?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 17: Trick Shot"
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput 1 "example1") )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput 1 "input") )

        yield ""
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart2 (this.GetPuzzleInput 2 "example1") )
        yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput 2 "input") )
        }