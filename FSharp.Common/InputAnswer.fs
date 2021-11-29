namespace FSharp.Common

open System
open System.Collections.Generic
open System.Diagnostics
open System.Linq
open System.Reflection

type InputAnswer<'TI, 'TA> (input: 'TI, expectedAnswer: 'TA) = 
    
    member this.Input = input
    member this.ExpectedAnswer = expectedAnswer