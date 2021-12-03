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
