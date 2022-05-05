namespace AdventOfCode.CSharp.Year2019.Tests;

public class Day07Tests : TestBase
{
    private readonly Day07 puzzle = new();
    public Day07Tests(ITestOutputHelper output) : base(output, 7) { }


    private (List<long>, List<long>?, long?) GetTestData(string name, int part)
    {
        var inputData = InputHelper.LoadInputFile(DAY, name)
            .ToList();

        var input = inputData[0]
            .Split(',')
            .Select(v => v.ToInt64())
            .ToList();

        List<long> phase = null;
        if (inputData.Count > 1)
        {
            phase = inputData[1]
                .Split(',')
                .Select(v => v.ToInt64())
                .ToList();
        }

        var expected = InputHelper.LoadAnswerFile(DAY, part, name)
            ?.FirstOrDefault()
            ?.ToInt64();

        return (input, phase, expected);
    }


    [Theory]
    [InlineData("example1")]
    [InlineData("example2")]
    [InlineData("example3")]
    [InlineData("input")]
    public void Part1(string inputName)
    {
        var (input, phase, expected) = GetTestData(inputName, 1);

        var answer = puzzle.RunPart1(input, phase);

        output.WriteLine($"Highest signal that can be sent to the thrusters: {answer}");

        Assert.Equal(expected, answer);
    }


    [Theory]
    [InlineData("example4")]
    [InlineData("example5")]
    [InlineData("input")]
    public void Part2(string inputName)
    {
        var (input, phase, expected) = GetTestData(inputName, 2);

        var answer = puzzle.RunPart2(input, phase);

        output.WriteLine($"Highest signal that can be sent to the thrusters: {answer}");

        Assert.Equal(expected, answer);
    }

}
