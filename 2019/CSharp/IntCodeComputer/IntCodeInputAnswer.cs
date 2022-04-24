namespace AdventOfCode.CSharp.Year2019.IntCodeComputer;

public class IntCodeInputAnswer<TA> : InputAnswer<List<string>, TA>
{
    public IntCodeInputAnswer() : base() { }
    public IntCodeInputAnswer(List<string> input, TA expectedAnswer) : base(input, expectedAnswer)
    {
    }

    protected override void SetInput(List<string> value)
    {
        base.SetInput(value);
        Code = Input.First()
            .Split(',')
            .Select(v => v.ToInt64())
            .ToList();
    }

    public List<long> Code { get; private set; }
}
