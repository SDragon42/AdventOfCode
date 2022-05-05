namespace AdventOfCode.CSharp.Year2019.Tests;

public class Day06Tests : TestBase
{
    private readonly Day06 puzzle = new();
    public Day06Tests(ITestOutputHelper output) : base(output, 6) { }


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
    [InlineData("input")]
    public void Part1(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var result = puzzle.RunPart1(input);

        output.WriteLine($"The total number of Direct and Indirect orbits: {result}");

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("example2")]
    [InlineData("input")]
    public void Part2(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 2);

        var result = puzzle.RunPart2(input);

        output.WriteLine($"The minimum # of orbit transfers required: {result}");

        Assert.Equal(expected, result);
    }
}
