namespace AdventOfCode.CSharp.Year2019.Tests;

public class Day03Tests : TestBase
{
    public Day03Tests(ITestOutputHelper output) : base(output, 3) { }

    private readonly Point origin = new Point(0, 0);

    private (List<string>, int?) GetTestData(string name, int part)
    {
        var input = InputHelper.LoadInputFile(DAY, name)
            .ToList();

        var expected = InputHelper.LoadAnswerFile(DAY, part, name)
            ?.FirstOrDefault()
            ?.ToInt32();

        return (input, expected);
    }

    [Theory]
    [InlineData("example1")]
    [InlineData("example2")]
    [InlineData("example3")]
    [InlineData("input")]
    public void Part1(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var puzzle = new Day03();
        var answer = puzzle.RunPart1(input);

        output.WriteLine($"Closest distance : {answer}");

        Assert.Equal(expected, answer);
    }

    [Theory]
    [InlineData("example4")]
    [InlineData("example5")]
    [InlineData("input")]
    public void Part2(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 2);

        var puzzle = new Day03();
        var answer = puzzle.RunPart2(input);

        output.WriteLine($"The fewest combined steps is : {answer}");

        Assert.Equal(expected, answer);
    }
}
