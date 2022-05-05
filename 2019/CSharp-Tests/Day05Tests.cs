namespace AdventOfCode.CSharp.Year2019.Tests;

public class Day05Tests : TestBase
{
    private readonly Day05 puzzle = new Day05();
    public Day05Tests(ITestOutputHelper output) : base(output, 5) { }


    private (List<long>, long?) GetTestData(string name, int part)
    {
        var input = InputHelper.LoadInputFile(DAY, name)
            .First()
            .Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        var expected = InputHelper.LoadAnswerFile(DAY, part, name)
            ?.FirstOrDefault()
            ?.ToInt64();

        return (input, expected);
    }

    [Theory]
    [InlineData("input", 1)]
    public void Part1(string inputName, long inputValue)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var answer = puzzle.RunPart1(input, inputValue);

        output.WriteLine($"The diagnostic Code is: {answer}");

        Assert.Equal(expected, answer);
    }
    [Theory]
    [InlineData("3,0,4,0,99", 69, 69)]
    [InlineData("1002,4,3,4,33", 1, 0)]
    public void Part1Example(string codeStr, long inputValue, long expected)
    {
        var input = codeStr.Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        var answer = puzzle.RunPart1(input, inputValue);

        output.WriteLine($"The diagnostic Code is: {answer}");

        Assert.Equal(expected, answer);
    }

    [Theory]
    [InlineData("input")]
    public void Part2(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 2);

        var answer = puzzle.RunPart2(input);

        output.WriteLine($"The diagnostic Code is: {answer}");

        Assert.Equal(expected, answer);
    }

    [Theory]
    [InlineData("3,9,8,9,10,9,4,9,99,-1,8", 8, 1)]
    [InlineData("3,9,7,9,10,9,4,9,99,-1,8", 8, 0)]
    [InlineData("3,3,1108,-1,8,3,4,3,99", 8, 1)]
    [InlineData("3,3,1107,-1,8,3,4,3,99", 8, 0)]
    [InlineData("3,9,8,9,10,9,4,9,99,-1,8", 7, 0)]
    [InlineData("3,9,7,9,10,9,4,9,99,-1,8", 7, 1)]
    [InlineData("3,3,1108,-1,8,3,4,3,99", 7, 0)]
    [InlineData("3,3,1107,-1,8,3,4,3,99", 7, 1)]
    public void Part2Example3(string codeStr, long value, long expected)
    {
        var input = codeStr.Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        var computer = new IntCode(input);
        var answer = 0L;
        computer.Output += (s, e) => answer = e.OutputValue;

        computer.Run();
        if (computer.State == IntCodeState.NeedsInput)
        {
            computer.AddInput(value);
            computer.Run();
        }

        Assert.Equal(expected, answer);
    }

    [Theory]
    [InlineData("3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9", 0, 0)]
    [InlineData("3,3,1105,-1,9,1101,0,0,12,4,12,99,1", 42, 1)]
    public void Part2Example4(string codeStr, long value, long expected)
    {
        var input = codeStr.Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        var computer = new IntCode(input);
        var answer = 0L;
        computer.Output += (s, e) => answer = e.OutputValue;

        computer.Run();
        if (computer.State == IntCodeState.NeedsInput)
        {
            computer.AddInput(value);
            computer.Run();
        }

        Assert.Equal(expected, answer);
    }
}
