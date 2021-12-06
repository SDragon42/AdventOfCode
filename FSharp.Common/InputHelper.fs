namespace FSharp.Common

open System.IO

module InputHelper =
    let LoadInputFile(day: int, name: string) =
        let filename = $@".\input\Day%02i{day}\%s{name}.txt";
        try
            File.ReadAllText(filename)
        with
            | ex -> null
