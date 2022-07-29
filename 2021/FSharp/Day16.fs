﻿module Day16

open FSharp.Common
open System

type private Packet (version, typeId) =
    member this.Version:int = version
    member this.TypeId:int = typeId

    [<DefaultValue>]
    val mutable Value:int64
    [<DefaultValue>]
    val mutable SubPackets:Packet list


type private PuzzleInput (input, expectedAnswer) =
    inherit InputAnswer<string, int64 option> (input, expectedAnswer)


type Day16 (runBenchmarks, runExamples) =
    inherit PuzzleBase (runBenchmarks, runExamples)


    member private this.GetPuzzleInput (part:int) (name: string) =
        let day = 16

        let input =
            InputHelper.LoadText (day, name)

        let answer = 
            InputHelper.LoadAnswer (day, $"%s{name}-answer%i{part}")
            |> InputHelper.AsInt64

        new PuzzleInput (input, answer)


    member private this.HexCharToBinaryString value =
        match value with
        | '0' -> "0000"
        | '1' -> "0001"
        | '2' -> "0010"
        | '3' -> "0011"
        | '4' -> "0100"
        | '5' -> "0101"
        | '6' -> "0110"
        | '7' -> "0111"
        | '8' -> "1000"
        | '9' -> "1001"
        | 'A' -> "1010"
        | 'B' -> "1011"
        | 'C' -> "1100"
        | 'D' -> "1101"
        | 'E' -> "1110"
        | 'F' -> "1111"
        | _ -> ""


    member private this.HexStringToBinaryString (value:string) =
        let result = 
            value
            |> Seq.map this.HexCharToBinaryString
            |> String.concat ""
        result


    member private this.GetPacketVersionType (input:string) =
        let version = input[0..2] |> Helper.BinaryStringToInt
        let typeId = input[3..5] |> Helper.BinaryStringToInt
        version, typeId, input[6..]

    member private this.GetPacketValue (partailValue:string) (input:string) = 
        let isPartial = input[0]
        let valuePart = input[1..4]
        let remainder = input[5..]
        match isPartial with
        | '0' ->
            let result = (partailValue + valuePart) |> Helper.BinaryStringToInt64
            result, remainder
        | _ ->
            this.GetPacketValue (partailValue + valuePart) remainder


    member private this.GetPacketLength (input:string) = 
        let pLengthTypeId = input[0]
        match pLengthTypeId with
        | '0' ->
            let dataLength = input[1..15] |> Helper.BinaryStringToInt
            pLengthTypeId, dataLength, input[16..]
        | _ -> 
            let dataLength = input[1..11] |> Helper.BinaryStringToInt
            pLengthTypeId, dataLength, input[12..]


    member private this.ParseToPackets (input:string) (subPacketsToProcess: int, bitsToProcess:int) =
        let version, typeId, inputRemainder = this.GetPacketVersionType(input)

        let packet = Packet (version, typeId)
        packet.Value <- 0
        packet.SubPackets <- []

        let HandleRemainingInput (inputRemainder:string) =
            let remainingBits = 
                match bitsToProcess with
                | _ when bitsToProcess > 0 -> bitsToProcess - (input.Length - inputRemainder.Length)
                | _ -> 0
            let packetsRemaining =
                match subPacketsToProcess with
                | _ when subPacketsToProcess > 0 -> subPacketsToProcess - 1
                | _ -> 0

            let moreToProcess = remainingBits > 0 || packetsRemaining > 0

            match moreToProcess with
            | true ->
                let otherPackets, inputRemainder = this.ParseToPackets inputRemainder (packetsRemaining, remainingBits)
                [packet] @ otherPackets, inputRemainder
            | false ->
                [packet], inputRemainder

        match packet.TypeId with
        | 4 -> // literal packet
            let literalValue, inputRemainder = this.GetPacketValue String.Empty inputRemainder
            packet.Value <- literalValue
            HandleRemainingInput inputRemainder
        | _ -> // Process sub packets
            let lengthTypeId, lengthValue, inputRemainder = this.GetPacketLength inputRemainder
            let packetLengthInfo =
                match lengthTypeId with
                | '0' -> 0, lengthValue // data length
                | _ -> lengthValue, 0 // Packet count

            let subPackets, inputRemainder = this.ParseToPackets inputRemainder packetLengthInfo
            packet.SubPackets <- subPackets
            HandleRemainingInput inputRemainder


    member private this.DecodeTransmition (input:string) =
        //let binInput = puzzleData.Input |> this.HexStringToBinaryString
        //let packets, _ = this.ParseToPackets binInput (1, 0)
        //let rootPacket = packets |> List.exactlyOne
        let rootPacket =
            input |> this.HexStringToBinaryString
            |> this.ParseToPackets <| (1, 0)
            |> (fun (pl, _) -> pl)
            |> List.exactlyOne
        rootPacket


    member private this.SumPacketVersions (packet:Packet) =
        let sumSubVersions = packet.SubPackets |> List.map this.SumPacketVersions |> List.sum
        packet.Version + sumSubVersions


    member private this.SumPackets (packet:Packet) =
        packet.SubPackets
            |> List.map this.EvaluateTransmission
            |> List.sum


    member private this.MultiplayPackets (packet:Packet) =
        packet.SubPackets
            |> List.map this.EvaluateTransmission
            |> List.fold (*) 1L


    member private this.MinimumPackets (packet:Packet) =
        packet.SubPackets
            |> List.map this.EvaluateTransmission
            |> List.min


    member private this.MaximumPackets (packet:Packet) =
        packet.SubPackets
            |> List.map this.EvaluateTransmission
            |> List.max


    member private this.GreaterThanPackets (packet:Packet) =
        let p0 = this.EvaluateTransmission packet.SubPackets[0]
        let p1 = this.EvaluateTransmission packet.SubPackets[1]
        p0 > p1 |> Convert.ToInt64


    member private this.LessThanPackets (packet:Packet) =
        let p0 = this.EvaluateTransmission packet.SubPackets[0]
        let p1 = this.EvaluateTransmission packet.SubPackets[1]
        p0 < p1 |> Convert.ToInt64


    member private this.EqualPackets (packet:Packet) =
        let p0 = this.EvaluateTransmission packet.SubPackets[0]
        let p1 = this.EvaluateTransmission packet.SubPackets[1]
        p0 = p1 |> Convert.ToInt64


    member private this.EvaluateTransmission (packet:Packet) =
        match packet.TypeId with
        | 0 -> this.SumPackets packet
        | 1 -> this.MultiplayPackets packet
        | 2 -> this.MinimumPackets packet
        | 3 -> this.MaximumPackets packet
        | 4 -> packet.Value
        | 5 -> this.GreaterThanPackets packet
        | 6 -> this.LessThanPackets packet
        | 7 -> this.EqualPackets packet
        | _ -> failwith $"Invalid Type ID (%i{packet.TypeId})"


    member private this.DisplayTransmition (packet:Packet) =
        let rec DoIt (p:Packet) (indent:string) =
            let value = this.EvaluateTransmission p
            let op =
                match p.TypeId with
                | 0 -> "(+)"
                | 1 -> "(*)"
                | 2 -> "MIN"
                | 3 -> "MAX"
                | 4 -> "VAL"
                | 5 -> "(>)"
                | 6 -> "(<)"
                | 7 -> "(=)"
                | _ -> ""
                + $"  =  {value}"
            let subs = p.SubPackets |> List.map (fun z -> DoIt z ("  " + indent)) |> List.concat
            [indent + op] @ subs

        let lines = DoIt packet ""
        printfn "%s" <| String.Join("\r\n", lines)

    member private this.RunPart1 (puzzleData: PuzzleInput) =
        let rootPacket = this.DecodeTransmition puzzleData.Input
        let result = this.SumPacketVersions rootPacket |> int64
        Helper.GetPuzzleResultText ("What do you get if you add up the version numbers in all packets?", result, puzzleData.ExpectedAnswer)


    member private this.RunPart2 (puzzleData: PuzzleInput) =
        let rootPacket = this.DecodeTransmition puzzleData.Input
        //this.DisplayTransmition rootPacket
        let result = this.EvaluateTransmission rootPacket
        Helper.GetPuzzleResultText ("What do you get if you evaluate the BITS transmission?", result, puzzleData.ExpectedAnswer)


    override this.SolvePuzzle _ = seq {
        yield "Day 16: Packet Decoder"
        yield this.RunExample (fun _ -> " Ex. 1) " + this.RunPart1 (this.GetPuzzleInput 1 "example01") )
        yield this.RunExample (fun _ -> " Ex. 2) " + this.RunPart1 (this.GetPuzzleInput 1 "example02") )
        yield this.RunExample (fun _ -> " Ex. 3) " + this.RunPart1 (this.GetPuzzleInput 1 "example03") )
        yield this.RunExample (fun _ -> " Ex. 4) " + this.RunPart1 (this.GetPuzzleInput 1 "example04") )
        yield this.RunProblem (fun _ -> "Part 1) " + this.RunPart1 (this.GetPuzzleInput 1 "input") )

        yield ""
        yield this.RunExample (fun _ -> " Ex. 5) " + this.RunPart2 (this.GetPuzzleInput 2 "example05") )
        yield this.RunExample (fun _ -> " Ex. 6) " + this.RunPart2 (this.GetPuzzleInput 2 "example06") )
        yield this.RunExample (fun _ -> " Ex. 7) " + this.RunPart2 (this.GetPuzzleInput 2 "example07") )
        yield this.RunExample (fun _ -> " Ex. 8) " + this.RunPart2 (this.GetPuzzleInput 2 "example08") )
        yield this.RunExample (fun _ -> " Ex. 9) " + this.RunPart2 (this.GetPuzzleInput 2 "example09") )
        yield this.RunExample (fun _ -> " Ex.10) " + this.RunPart2 (this.GetPuzzleInput 2 "example10") )
        yield this.RunExample (fun _ -> " Ex.11) " + this.RunPart2 (this.GetPuzzleInput 2 "example11") )
        yield this.RunExample (fun _ -> " Ex.12) " + this.RunPart2 (this.GetPuzzleInput 2 "example12") )
        yield this.RunProblem (fun _ -> "Part 2) " + this.RunPart2 (this.GetPuzzleInput 2 "input") )
        }