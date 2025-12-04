namespace FSharp.Common

open System.IO

module InputHelper =

    let Filename (year: int, day: int, name: string) =
        $@"../../../../../../AdventOfCode.Input/%i{year}/Day%02i{day}/%s{name}.txt";


    let LoadText = 
        Filename >> File.ReadAllText


    let LoadLines = 
        Filename >> File.ReadLines


    let LoadAnswer (year: int, day: int, name: string) =
        try LoadText(year, day, name) |> Some
        with | ex -> None


    let AsType(text: string option, typeFunc) =
        try text.Value |> typeFunc |> Some
        with | ex -> None


    let AsInt(text: string option) = AsType(text, int)


    let AsInt64(text: string option) = AsType(text, int64)


    let AsUInt64(text: string option) = AsType(text, uint64)
