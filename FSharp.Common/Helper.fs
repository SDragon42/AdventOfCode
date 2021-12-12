namespace FSharp.Common

open System

module Helper =

    let GetPuzzleResultText (message: string, foundAnswer, expectedAnswer) =
        let answerCheckText = 
            match expectedAnswer with
            | None -> String.Empty
            | _ when foundAnswer.Equals(expectedAnswer.Value) -> "CORRECT"
            | _ -> "WRONG"
        $"{message}    {foundAnswer}    {answerCheckText}".TrimEnd()


    let IsUpper (str : string) =
        let rec strIter isUpper arr =
            match arr with
            | [] -> isUpper
            | _ -> 
                match Char.IsLower(arr.Head) with
                | true -> false //strIter false []
                | false -> strIter true arr.Tail

        strIter true (Array.toList <| str.ToCharArray())


    let IsLower (str : string) =
        not <| IsUpper(str)