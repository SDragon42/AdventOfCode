namespace AdventOfCode.CSharp.Year2022;

public class Day18_Boiling_Boulders
{
    private const int DAY = 18;

    private readonly ITestOutputHelper output;
    public Day18_Boiling_Boulders(ITestOutputHelper output) => this.output = output;



    private (List<int> input, int? expected) GetTestData(int part, string inputName)
    {
        var input = InputHelper.ReadLines(DAY, inputName)
            .Select(l => l.ToInt32())
            .ToList();

        var expected = InputHelper.ReadText(DAY, $"{inputName}-answer{part}")
            ?.ToInt32();

        return (input, expected);
    }



    [Theory]
    [InlineData(1, "example1")]
    [InlineData(1, "input")]
    public void Part1(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = DoWork(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }


    [Theory]
    [InlineData(2, "example1")]
    [InlineData(2, "input")]
    public void Part2(int part, string inputName)
    {
        var (input, expected) = GetTestData(part, inputName);

        var value = DoWork(input);

        output.WriteLine($"Answer: {value}");

        Assert.Equal(expected, value);
    }



    private int DoWork(List<int> input)
    {
        return -1;
    }
}