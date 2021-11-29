namespace FSharp.Common

open System
open System.IO

module InputHelper =
    let LoadInputFile(day: int, name: string) =
        let filename = $@".\input\Day%02i{day}\%s{name}.txt";
        try
            File.ReadAllText(filename)
        with
            | ex -> null

    //let LoadInputFile (day: int, name: string) =
    //    let filename = $@".\input\Day%02i{day}\%s{name}.txt";
    //    File.ReadLines(filename)


    //let LoadAnswerFile (day: int, part: int, name: string)  =
    //    let filename = $@".\input\Day%02i{day}\%s{name}-answer%i{part}.txt";
    //    try
    //        File.ReadLines(filename)
    //    with
    //        | ex -> null

    let parseEachLine f = File.ReadLines >> Seq.map f
