namespace AdventOfCode.CSharp.Year2019.Tests;

public class Day01Tests : TestBase
{
    public Day01Tests(ITestOutputHelper output) : base(output, 1) { }

    List<int> GetInput(string name)
    {
        var input = InputHelper.LoadInputFile(DAY, name)
            .Select(l => l.ToInt32())
            .ToList();
        return input;
    }
    int? GetExpectedAnswer(int part, string name)
    {
        var expected = InputHelper.LoadAnswerFile(DAY, part, name)
            ?.FirstOrDefault()
            ?.ToInt32();
        return expected;
    }


    [Theory]
    [InlineData("example1")]
    [InlineData("example2")]
    [InlineData("example3")]
    [InlineData("example4")]
    [InlineData("input")]
    public void Part1(string inputName)
    {
        var puzzle = new Day01();

        var input = GetInput(inputName);
        var expected = GetExpectedAnswer(1, inputName);

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
        var puzzle = new Day01();

        var input = GetInput(inputName);
        var expected = GetExpectedAnswer(2, inputName);

        var value = puzzle.CalcTotalFuelForMasses(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


}
