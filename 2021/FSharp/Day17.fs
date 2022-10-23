// It took along time for me to realize part 1 required no code... 
// The answer is just n(n + 1)/2 where n = -target_min_y - 1.
namespace AdventOfCode.FSharp.Year2021

open FSharp.Common
open System
open System.Text.RegularExpressions
open Xunit



module ``Day 17: Trick Shot`` =
    let day = 17



    //-------------------------------------------------------------------------



    type private Area (xLow:int, xHigh:int, yLow:int, yHigh:int) =
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



    //-------------------------------------------------------------------------



    type private Puzzle () =

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


        // What is the highest y position it reaches on this trajectory?
        member this.RunPart1 (input:Area) =
            let minXVelocity = this.FindMinXVelocity input.XLow 0
            let maxYVelocity = this.FindMaxYVelocity input.YLow

            let initalVelocity = (minXVelocity, maxYVelocity)
            let success, points = this.ProcessStep input (0,0) initalVelocity
        
            let result =
                match success with
                | true -> points |> List.map (fun (x,y) -> y) |> List.max
                | false -> -9999
            result


        // How many distinct initial velocity values cause the probe to be within the target area?
        member this.RunPart2 (input:Area) =
            let minXVelocity = this.FindMinXVelocity input.XLow 0
            let maxXVelocity = input.XHigh
            let minYVelocity = input.YLow
            let maxYVelocity = this.FindMaxYVelocity input.YLow

            let xRange = [minXVelocity..maxXVelocity]
            let yRange = [minYVelocity..maxYVelocity]

            let allVelocities = 
                yRange
                |> List.map (fun y -> xRange |> List.map (fun x -> (x,y)))
                |> List.concat
                |> List.map (fun vel -> this.InTargetArea input vel)
                |> List.filter (fun (success, _) -> success)
                |> List.map (fun (_, vel) -> vel)
                |> List.distinct

            let result = allVelocities.Length
            result


        
    //-------------------------------------------------------------------------
        
        
    let private GetPuzzleInput (part:int) (name:string) =
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
                