namespace FSharp.Common

open System.IO

module InputHelper =

    let Filename (day: int, name: string) =
        $@".\input\Day%02i{day}\%s{name}.txt";

    let LoadText = 
        Filename >> File.ReadAllText

    let LoadLines = 
        Filename >> File.ReadLines

    let LoadAnswer (day: int, name: string) =
        try LoadText(day, name) |> Some
        with | ex -> None

    let AsType(text: string option, typeFunc) =
        try text.Value |> typeFunc |> Some
        with | ex -> None

    let AsInt(text: string option) = AsType(text, int)
    let AsInt64(text: string option) = AsType(text, int64)
    let AsUInt64(text: string option) = AsType(text, uint64)
