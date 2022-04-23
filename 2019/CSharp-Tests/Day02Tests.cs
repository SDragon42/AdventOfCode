namespace AdventOfCode.CSharp.Year2019.Tests;

public class Day02Tests : TestBase
{
    public Day02Tests(ITestOutputHelper output) : base(output, 2) { }

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
    [InlineData("example1", -1, -1)]
    [InlineData("input", 12, 2)]
    public void Part1(string inputName, int valueAt1, int valueAt2)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var puzzle = new Day02();
        var value = puzzle.RunCode(input, valueAt1, valueAt2);

        output.WriteLine($"Value as position 0 : {value}");

        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData("input", 19690720)]
    public void Part2(string inputName, long valueAt0)
    {
        var (input, expected) = GetTestData(inputName, 2);

        var puzzle = new Day02();
        var value = puzzle.FindNounVerb(input, valueAt0);

        output.WriteLine($"Noun-Verb pair is : {value}");

        Assert.Equal(expected, value);
    }
}
