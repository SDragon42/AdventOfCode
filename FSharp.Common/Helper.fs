namespace FSharp.Common

open System

module Helper =

    let GetPuzzleResultText (message:string, foundAnswer, expectedAnswer) =
        let answerCheckText = 
            match expectedAnswer with
            | None -> String.Empty
            | _ when foundAnswer.Equals(expectedAnswer.Value) -> "CORRECT"
            | _ -> "WRONG"
        $"{message}    {foundAnswer}    {answerCheckText}".TrimEnd()


    let IsLower (value:string) =
        value |> Seq.forall (fun c -> Char.IsLower c)


    let IsUpper = IsLower >> not


    let BinaryStringToInt value =
        Convert.ToInt32 (value, 2)


    let BinaryStringToInt64 value =
        Convert.ToInt64 (value, 2)

