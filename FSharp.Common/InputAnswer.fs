namespace FSharp.Common

open System
open System.Collections.Generic
open System.Diagnostics
open System.Linq
open System.Reflection

type InputAnswer<'TI, 'TA> (input: 'TI, expectedAnswer: 'TA) = 
    
    member this.Input = input
    member this.ExpectedAnswer = expectedAnswer


//public class InputAnswer<TI, TA>
//{
//    public InputAnswer() { }
//    public InputAnswer(TI input, TA expectedAnswer)
//    {
//        Input = input;
//        ExpectedAnswer = expectedAnswer;
//    }

//    private TI _input;
//    public TI Input
//    {
//        get => _input;
//        set => SetInput(value);
//    }
//    protected virtual void SetInput(TI value)
//    {
//        _input = value;
//    }

//    private TA _expectedAnswer;
//    public TA ExpectedAnswer
//    {
//        get => _expectedAnswer;
//        set => SetExpectedAnswer(value);
//    }
//    protected virtual void SetExpectedAnswer(TA value)
//    {
//        _expectedAnswer = value;
//    }
//}