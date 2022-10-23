namespace FSharp.Common

open System

module Helper =

    let IsLower (value:string) =
        value |> Seq.forall (fun c -> Char.IsLower c)


    let IsUpper = IsLower >> not


    let BinaryStringToInt value =
        Convert.ToInt32 (value, 2)


    let BinaryStringToInt64 value =
        Convert.ToInt64 (value, 2)

