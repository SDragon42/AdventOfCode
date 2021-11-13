namespace AdventOfCode.CSharp.Year2019.IntCodeComputer;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
class OpCodeAttribute : Attribute
{
    public OpCodeAttribute(int opCode)
    {
        OpCode = opCode;
    }

    public int OpCode { get; private set; }
}
