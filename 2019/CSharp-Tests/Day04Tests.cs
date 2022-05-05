namespace AdventOfCode.CSharp.Year2019.Tests;

public class Day04Tests : TestBase
{
    private readonly Day04 puzzle = new();

    public Day04Tests(ITestOutputHelper output) : base(output, 4) { }

    private (List<int>, int?) GetTestData(string name, int part)
    {
        var input = InputHelper.LoadInputFile(DAY, name)
            .First()
            .Split('-')
            .Select(l => l.ToInt32())
            .ToList();

        var expected = InputHelper.LoadAnswerFile(DAY, part, name)
            ?.FirstOrDefault()
            ?.ToInt32();

        return (input, expected);
    }

    [Theory]
    [InlineData("input")]
    public void Part1(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var answer = puzzle.RunPart1(input);

        output.WriteLine($"Closest distance : {answer}");

        Assert.Equal(expected, answer);
    }

    [Theory]
    [InlineData("input")]
    public void Part2(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 2);

        var answer = puzzle.RunPart2(input);

        output.WriteLine($"The fewest combined steps is : {answer}");

        Assert.Equal(expected, answer);
    }
}
