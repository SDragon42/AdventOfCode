namespace FSharp.Common

open System

module Helper =

    let GetPuzzleResultText2 (message: string, foundAnswer: string, expectedAnswer: string) =
        let answerCheckText = 
            if String.IsNullOrEmpty(expectedAnswer) then
                String.Empty
            else
                let result =
                    if foundAnswer.Equals(expectedAnswer) then
                        "CORRECT"
                    else
                        "WRONG"
                "    " + result
        message + answerCheckText

    let GetPuzzleResultText (message: string, foundAnswer, expectedAnswer) =
        GetPuzzleResultText2(message, foundAnswer.ToString(), expectedAnswer.ToString())
