namespace AdventOfCode.CSharp.Year2019.Tests;

public class Day01Tests : TestBase
{
    public Day01Tests(ITestOutputHelper output) : base(output, 1) { }

    private (List<int>, int?) GetTestData(string name, int part)
    {
        var input = InputHelper.LoadInputFile(DAY, name)
            .Select(l => l.ToInt32())
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
    [InlineData("example4")]
    [InlineData("input")]
    public void Part1(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 1);

        var puzzle = new Day01();
        var value = puzzle.CalcFuelForMasses(input);

        output.WriteLine($"Answer: {value}");
        
        Assert.Equal(expected, value);
    }

    [Theory]
    [InlineData("example2")]
    [InlineData("example3")]
    [InlineData("input")]
    public void Part2(string inputName)
    {
        var (input, expected) = GetTestData(inputName, 2);

        var puzzle = new Day01();
        var value = puzzle.CalcTotalFuelForMasses(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


}
