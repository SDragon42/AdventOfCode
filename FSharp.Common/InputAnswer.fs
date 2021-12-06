namespace FSharp.Common

type InputAnswer<'TI, 'TA> (input: 'TI, expectedAnswer: 'TA) = 
    
    member this.Input = input
    member this.ExpectedAnswer = expectedAnswer